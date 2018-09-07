using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public sealed class InstanceMemberAttribute : Attribute
    {
    }
}
