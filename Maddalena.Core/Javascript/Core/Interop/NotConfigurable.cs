using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
    public sealed class NotConfigurable : Attribute
    {
    }
}
