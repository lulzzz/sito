﻿using System;

namespace JS.Core.Core.Interop
{
    /// <summary>
    /// Член, помеченный данным аттрибутом, не будет удаляться оператором "delete".
    /// </summary>
#if !(PORTABLE)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class DoNotDeleteAttribute : Attribute
    {
    }
}
