﻿using System;
using JS.Core.Core;
using JS.Core.Core.Interop;

namespace NiL.JS.BaseLibrary
{
    [Prototype(typeof(Error))]
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class SyntaxError : Error
    {
        [DoNotEnumerate]
        public SyntaxError()
        {

        }

        [DoNotEnumerate]
        public SyntaxError(Arguments args)
            : base(args[0].ToString())
        {

        }

        [DoNotEnumerate]
        public SyntaxError(string message)
            : base(message)
        {

        }
    }
}
