using System;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    [Prototype(typeof(Error))]
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ReferenceError : Error
    {
        [DoNotEnumerate]
        public ReferenceError(Arguments args)
            : base(args[0].ToString())
        {

        }

        [DoNotEnumerate]
        public ReferenceError()
        {

        }

        [DoNotEnumerate]
        public ReferenceError(string message)
            : base(message)
        {
        }
    }
}
