using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
    public sealed class ToStringTagAttribute : Attribute
    {
        public string Tag { get; }

        public ToStringTagAttribute(string tag)
        {
            Tag = tag;
        }
    }
}
