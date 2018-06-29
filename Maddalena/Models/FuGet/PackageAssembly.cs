using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using Maddalena.Models.FuGet.Extensions;
using Mono.Cecil;

namespace Maddalena.Models.FuGet
{
    public class PackageAssembly : PackageFile
    {
        private readonly Lazy<CSharpDecompiler> decompiler;
        private readonly Lazy<AssemblyDefinition> definition;

        private readonly CSharpFormattingOptions format;
        private readonly PackageTargetFramework framework;
        private readonly Lazy<CSharpDecompiler> idecompiler;

        private readonly ConcurrentDictionary<TypeDefinition, TypeDocumentation> typeDocs =
            new ConcurrentDictionary<TypeDefinition, TypeDocumentation>();

        public PackageAssembly(ZipArchiveEntry entry, PackageTargetFramework framework)
            : base(entry)
        {
            this.framework = framework;
            var ms = new MemoryStream((int) ArchiveEntry.Length);
            using (var es = entry.Open())
            {
                es.CopyTo(ms);
                ms.Position = 0;
            }

            definition = new Lazy<AssemblyDefinition>(() =>
            {
                try
                {
                    return AssemblyDefinition.ReadAssembly(ms, new ReaderParameters
                    {
                        AssemblyResolver = framework.AssemblyResolver
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to load assembly");
                    Debug.WriteLine(ex);
                    return null;
                }
            }, true);
            format = FormattingOptionsFactory.CreateMono();
            format.SpaceBeforeMethodCallParentheses = false;
            format.SpaceBeforeMethodDeclarationParentheses = false;
            format.SpaceBeforeConstructorDeclarationParentheses = false;
            format.PropertyBraceStyle = BraceStyle.EndOfLine;
            format.PropertyGetBraceStyle = BraceStyle.EndOfLine;
            format.PropertySetBraceStyle = BraceStyle.EndOfLine;
            format.AutoPropertyFormatting = PropertyFormatting.ForceOneLine;
            format.SimplePropertyFormatting = PropertyFormatting.ForceOneLine;
            format.IndentPropertyBody = false;
            format.IndexerDeclarationClosingBracketOnNewLine = NewLinePlacement.SameLine;
            format.IndexerClosingBracketOnNewLine = NewLinePlacement.SameLine;
            format.NewLineAferIndexerDeclarationOpenBracket = NewLinePlacement.SameLine;
            format.NewLineAferIndexerOpenBracket = NewLinePlacement.SameLine;

            idecompiler = new Lazy<CSharpDecompiler>(() =>
            {
                var m = Definition?.MainModule;
                if (m == null)
                    return null;
                return new CSharpDecompiler(m, new DecompilerSettings
                {
                    ShowXmlDocumentation = false,
                    ThrowOnAssemblyResolveErrors = false,
                    AlwaysUseBraces = false,
                    CSharpFormattingOptions = format,
                    ExpandMemberDefinitions = false,
                    DecompileMemberBodies = false,
                    UseExpressionBodyForCalculatedGetterOnlyProperties = true
                });
            }, true);
            decompiler = new Lazy<CSharpDecompiler>(() =>
            {
                var m = Definition?.MainModule;
                if (m == null)
                    return null;
                return new CSharpDecompiler(m, new DecompilerSettings
                {
                    ShowXmlDocumentation = false,
                    ThrowOnAssemblyResolveErrors = false,
                    AlwaysUseBraces = false,
                    CSharpFormattingOptions = format,
                    ExpandMemberDefinitions = true,
                    DecompileMemberBodies = true,
                    UseExpressionBodyForCalculatedGetterOnlyProperties = true
                });
            }, true);
        }

        public PackageAssemblyXmlDocs XmlDocs
        {
            get
            {
                framework.AssemblyXmlDocs.TryGetValue(Definition.Name.Name, out var docs);
                return docs;
            }
        }

        public AssemblyDefinition Definition => definition.Value;

        public bool IsBuildAssembly =>
            ArchiveEntry.FullName.StartsWith("build/", StringComparison.InvariantCultureIgnoreCase);

        public bool IsPublic => Definition.Modules.Any(m => m.Types.Any(t => t.IsPublic));

        public IEnumerable<TypeDefinition> PublicTypes => Definition.Modules
            .SelectMany(m => m.Types.Where(t => t.IsPublic)).OrderBy(x => x.FullName);

        public override string ToString()
        {
            return Definition.Name.Name;
        }

        public TypeDocumentation GetTypeDocumentation(TypeDefinition typeDefinition)
        {
            if (typeDocs.TryGetValue(typeDefinition, out var docs)) return docs;
            var asmName = typeDefinition.Module.Assembly.Name.Name;
            docs = new TypeDocumentation(typeDefinition, framework, XmlDocs, decompiler, idecompiler, format);
            typeDocs.TryAdd(typeDefinition, docs);
            return docs;
        }

        public void Search(string query, InPackageSearchResults r)
        {
            if (Definition == null)
                return;
            foreach (var m in Definition.Modules)
                Parallel.ForEach(m.Types, t => { SearchType(t, query, r); });
        }

        private void SearchType(TypeDefinition d, string query, InPackageSearchResults r)
        {
            if (d.Name == "<PrivateImplementationDetails>")
                return;
            var ds = NameScore(d.Name, query);
            if (ds != int.MinValue)
                r.Add(framework, this, d, d.GetFriendlyName(), d.Namespace, "type", d.Name, d.IsPublic, ds);

            foreach (var x in d.Methods)
                SearchMethod(d, x, query, r);
            foreach (var x in d.Properties)
                SearchProperty(d, x, query, r);
            foreach (var x in d.Events)
                SearchEvent(d, x, query, r);
            foreach (var x in d.Fields)
                SearchField(d, x, query, r);
        }

        private void SearchMethod(TypeDefinition t, MethodDefinition d, string query, InPackageSearchResults r)
        {
            var name = d.Name;
            var nd = name.LastIndexOf('.');
            if (nd > 0 && nd + 1 < name.Length) name = name.Substring(nd + 1);
            if (d.IsAddOn || d.IsRemoveOn || d.IsGetter || d.IsSetter)
                return;

            var ds = NameScore(name, query);
            if (ds != int.MinValue)
                r.Add(framework, this, t, name, d.DeclaringType.Name, "method", d.Name, d.IsPublic, ds);
        }

        private void SearchProperty(TypeDefinition t, PropertyDefinition d, string query, InPackageSearchResults r)
        {
            var name = d.Name;
            var nd = name.LastIndexOf('.');
            if (nd > 0 && nd + 1 < name.Length) name = name.Substring(nd + 1);
            var ds = NameScore(name, query);
            if (ds != int.MinValue)
                r.Add(framework, this, t, name, d.DeclaringType.Name, "property", d.Name,
                    d.GetMethod != null && d.GetMethod.IsPublic, ds);
        }

        private void SearchField(TypeDefinition t, FieldDefinition d, string query, InPackageSearchResults r)
        {
            if (d.Name.IndexOf("k__BackingField", StringComparison.InvariantCulture) >= 0)
                return;

            var ds = NameScore(d.Name, query);
            if (ds != int.MinValue)
                r.Add(framework, this, t, d.Name, d.DeclaringType.Name, "field", d.Name, d.IsPublic, ds);
        }

        private void SearchEvent(TypeDefinition t, EventDefinition d, string query, InPackageSearchResults r)
        {
            var name = d.Name;
            var nd = name.LastIndexOf('.');
            if (nd > 0 && nd + 1 < name.Length) name = name.Substring(nd + 1);
            var ds = NameScore(name, query);
            if (ds != int.MinValue)
                r.Add(framework, this, t, name, d.DeclaringType.Name, "event", d.Name,
                    d.AddMethod != null && d.AddMethod.IsPublic, ds);
        }

        private int NameScore(string name, string query)
        {
            var s = 0;
            var ni = 0;
            var qi = 0;
            var needsCap = true;
            while (qi < query.Length && ni < name.Length)
            {
                var n = name[ni];
                var q = query[qi];
                var un = char.ToUpperInvariant(n);
                var uq = char.ToUpperInvariant(q);
                if (needsCap && (n == uq || ni == 0 && un == uq))
                {
                    s++;
                    if (ni == 0 && qi == 0) s += 1000;
                    ni++;
                    qi++;
                    needsCap = false;
                }
                else if (!needsCap && un == uq)
                {
                    s++;
                    ni++;
                    qi++;
                }
                else
                {
                    s--;
                    ni++;
                    needsCap = true;
                }
            }

            if (qi != query.Length) return int.MinValue;
            s -= name.Length - ni;
            return s;
        }
    }
}