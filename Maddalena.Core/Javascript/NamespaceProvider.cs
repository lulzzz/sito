using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript
{
    /// <summary>
    /// Provides access to a CLR-namespace
    /// </summary>
    [Serializable]
    public class NamespaceProvider : CustomType
    {
        private static readonly BinaryTree<Type> types = new BinaryTree<Type>();

        private static void addTypes(Assembly assembly)
        {
            try
            {
                if (assembly is AssemblyBuilder)
                    return;
                var types = assembly.GetExportedTypes();
                for (var i = 0; i < types.Length; i++)
                {
                    NamespaceProvider.types[types[i].FullName] = types[i];
                }
            }
            catch
            {

            }
        }

        static NamespaceProvider()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                addTypes(assembly);
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            addTypes(args.LoadedAssembly);
        }

        private BinaryTree<JSValue> childs;

        /// <summary>
        /// Contract NamespacesProvider
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Contract NamespacesProvider
        /// </summary>
        /// <param name="namespace">Namespace</param>
        public NamespaceProvider(string @namespace)
        {
            Namespace = @namespace;
        }

        protected internal override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope memberScope)
        {
            if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
            {
                var name = key.ToString();
                JSValue res = null;
                if (childs != null && childs.TryGetValue(name, out res))
                    return res;
                string reqname = Namespace + "." + name;
                var selection = types.StartsWith(reqname).GetEnumerator();

                Type resultType = null;
                List<Type> ut = null;

                while (selection.MoveNext())
                {
                    if (selection.Current.Value.FullName.Length > reqname.Length 
                        && selection.Current.Value.FullName[reqname.Length] == '`')
                    {
                        string fn = selection.Current.Value.FullName;
                        for (var i = fn.Length - 1; i > reqname.Length; i--)
                        {
                            if (!Tools.IsDigit(fn[i]))
                            {
                                fn = null;
                                break;
                            }
                        }

                        if (fn != null)
                        {
                            if (resultType == null)
                            {
                                resultType = selection.Current.Value;
                            }
                            else
                            {
                                if (ut == null)
                                    ut = new List<Type> { resultType };

                                ut.Add(selection.Current.Value);
                            }
                        }
                    }
                    else if (selection.Current.Value.Name != name)
                        break;
                    else
                        resultType = selection.Current.Value;
                }

                if (ut != null)
                {
                    res = GetGenericTypeSelector(ut);

                    if (childs == null)
                        childs = new BinaryTree<JSValue>();

                    childs[name] = res;
                    return res;
                }

                if (resultType != null)
                    return Context.CurrentGlobalContext.GetConstructor(resultType);

                selection = types.StartsWith(reqname).GetEnumerator();
                if (selection.MoveNext() && selection.Current.Key[reqname.Length] == '.')
                {
                    res = new NamespaceProvider(reqname);

                    if (childs == null)
                        childs = new BinaryTree<JSValue>();

                    childs.Add(name, res);
                    return res;
                }
            }

            return undefined;
        }

        public static Type GetType(string name)
        {
            var selection = types.StartsWith(name).GetEnumerator();
            if (selection.MoveNext() && selection.Current.Key == name)
                return selection.Current.Value;
            return null;
        }

        public static IEnumerable<Type> GetTypesByPrefix(string prefix)
        {
            return types.StartsWith(prefix).Select(type => type.Value);
        }

        protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool pdef, EnumerationMode enumerationMode)
        {
            yield break;
        }
    }
}
