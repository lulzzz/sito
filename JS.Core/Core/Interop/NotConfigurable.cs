﻿using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = false)]
    public sealed class NotConfigurable : Attribute
    {
    }
}
