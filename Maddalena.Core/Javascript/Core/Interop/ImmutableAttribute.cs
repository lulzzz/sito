﻿using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    /// <summary>
    /// Объект-прослойка, созданный для типа, помеченного данным аттрибутом, 
    /// не будет допускать создание полей, которые не существуют в помеченном типе.
    /// </summary>
#if !(PORTABLE)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class ImmutableAttribute : Attribute
    {
    }
}
