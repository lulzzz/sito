using System;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    [Prototype(typeof(Error))]
    [Serializable]
    public sealed class TypeError : Error
    {
        [DoNotEnumerate]
        public TypeError(Arguments args)
            : base(args[0].ToString())
        {

        }

        [DoNotEnumerate]
        public TypeError()
        {

        }

        [DoNotEnumerate]
        public TypeError(string message)
            : base(message)
        {
        }
    }
}
