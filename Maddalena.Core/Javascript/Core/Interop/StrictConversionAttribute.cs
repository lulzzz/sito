using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate, 
        Inherited = false)]
    public sealed class StrictConversionAttribute : Attribute
    {
    }
}
