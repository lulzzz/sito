using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Extensions
{
    internal sealed class EnumeratorResult : IterableProtocolBase, IIteratorResult
    {
        public JSValue value { get; }

        public bool done { get; }

        [Hidden]
        public EnumeratorResult(bool done, JSValue value)
        {
            this.value = value;
            this.done = done;
        }
    }
}