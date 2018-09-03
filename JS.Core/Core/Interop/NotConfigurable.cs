using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
    public sealed class NotConfigurable : Attribute
    {
    }
}
