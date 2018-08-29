using JS.Core.Core;
using JS.Core.Core.Interop;

namespace JS.Core.Extensions
{
    internal sealed class IteratorItemAdapter : IterableProtocolBase, IIteratorResult
    {
        private readonly JSValue result;

        [Hidden]
        public IteratorItemAdapter(JSValue result)
        {
            this.result = result;
        }

        public JSValue value => Tools.InvokeGetter(result["value"], result);

        public bool done => (bool)Tools.InvokeGetter(result["done"], result);
    }
}