using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class JavaScriptNameAttribute : Attribute
    {
        public string Name { get; }

        public JavaScriptNameAttribute(string name)
        {
            Name = name;
        }
    }
}
