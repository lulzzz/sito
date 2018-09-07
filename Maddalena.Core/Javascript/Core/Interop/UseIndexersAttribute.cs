using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class UseIndexersAttribute : Attribute
    {
    }
}
