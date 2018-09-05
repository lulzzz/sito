﻿using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
    public sealed class ToStringTagAttribute : Attribute
    {
        public string Tag { get; }

        public ToStringTagAttribute(string tag)
        {
            Tag = tag;
        }
    }
}