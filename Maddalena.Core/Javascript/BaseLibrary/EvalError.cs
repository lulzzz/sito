using System;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.BaseLibrary
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class EvalError : Error
    {
        [DoNotEnumerate]
        public EvalError()
        {

        }

        [DoNotEnumerate]
        public EvalError(Arguments args)
            : base(args[0].ToString())
        {

        }

        [DoNotEnumerate]
        public EvalError(string message)
            : base(message)
        {

        }
    }
}
