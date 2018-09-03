using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class DisallowNewKeywordAttribute : Attribute
    {
    }
}
