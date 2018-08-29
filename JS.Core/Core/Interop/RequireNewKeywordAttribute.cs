using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class RequireNewKeywordAttribute : Attribute
    {
    }
}
