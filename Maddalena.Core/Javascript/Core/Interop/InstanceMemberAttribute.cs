using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public sealed class InstanceMemberAttribute : Attribute
    {
    }
}
