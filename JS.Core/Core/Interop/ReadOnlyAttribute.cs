﻿using System;

namespace JS.Core.Core.Interop
{
    /// <summary>
    /// Значение поля, помеченного данным аттрибутом, будет неизменяемо для скрипта.
    /// </summary>
#if !(PORTABLE)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ReadOnlyAttribute : Attribute
    {

    }
}
