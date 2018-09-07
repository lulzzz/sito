using JS.Core.Core;
using JS.Core.Core.Interop;

namespace JS.Core.Extensions
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