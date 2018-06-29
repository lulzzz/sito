using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Xml.Linq;

namespace Maddalena.Models.FuGet
{
    public class PackageAssemblyXmlDocs
    {
        private readonly XDocument doc;

        public PackageAssemblyXmlDocs(ZipArchiveEntry entry)
        {
            try
            {
                using (var s = entry.Open())
                {
                    doc = XDocument.Load(s);
                    var ns = doc.Root.Name.Namespace;
                    var asm = doc.Root.Element(ns + "assembly");
                    if (asm == null)
                    {
                        Error = "Not an docs files";
                        return;
                    }

                    AssemblyName = asm.Value.Trim();
                    // System.Console.WriteLine(AssemblyName);
                    foreach (var me in doc.Root.Element(ns + "members").Elements(ns + "member"))
                    {
                        var m = new MemberXmlDocs(me);
                        // System.Console.WriteLine(m.Name);
                        if (!string.IsNullOrEmpty(m.Name)) MemberDocs[m.Name] = m;
                    }
                }

                Error = "";
            }
            catch (Exception ex)
            {
                // System.Console.WriteLine (ex);
                // if (doc != null) System.Console.WriteLine(doc);
                Error = ex.Message;
            }
        }

        public string Error { get; }

        public string AssemblyName { get; }

        public Dictionary<string, MemberXmlDocs> MemberDocs { get; } = new Dictionary<string, MemberXmlDocs>();
    }

    public class MemberXmlDocs
    {
        public MemberXmlDocs(XElement memberElement)
        {
            var ns = memberElement.Name.Namespace;
            var na = memberElement.Attribute("name");
            if (na != null && !string.IsNullOrWhiteSpace(na.Value))
                Name = na.Value;
            else
                Name = "";
            SummaryXml = memberElement.Element(ns + "summary");
            // System.Console.WriteLine(SummaryXml.ToString());
        }

        public string Name { get; }
        public XElement SummaryXml { get; }

        public string SummaryText => SummaryXml != null ? SummaryXml.Value : "";
    }
}