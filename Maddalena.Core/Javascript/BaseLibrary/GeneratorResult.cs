using System;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    [Serializable]
    public sealed class GeneratorResult : IIteratorResult
    {
        [Field]
        public JSValue value { get; }

        [Field]
        public bool done { get; }

        [Hidden]
        public GeneratorResult(JSValue value, bool done)
        {
            this.value = value;
            this.done = done;
        }
    }
}