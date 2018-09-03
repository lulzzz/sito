using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate, 
        Inherited = false)]
    public sealed class StrictConversionAttribute : Attribute
    {
    }
}
