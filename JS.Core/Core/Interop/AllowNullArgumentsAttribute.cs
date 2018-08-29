﻿using System;

namespace JS.Core.Core.Interop
{
    /// <summary>
    /// For compatibility with legacy code.
    /// Specifies that method will work correctly if container of arguments will be equal to null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed class AllowNullArgumentsAttribute : Attribute
    {
    }
}
