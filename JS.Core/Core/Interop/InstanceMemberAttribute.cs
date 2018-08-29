using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class InstanceMemberAttribute : Attribute
    {
    }
}
