﻿using System;

namespace JS.Core.Core.Interop
{
    /// <summary>
    /// Член, помеченный данным аттрибутом, не будет удаляться оператором "delete".
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false)]
    public sealed class DoNotDeleteAttribute : Attribute
    {
    }
}
