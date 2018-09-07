using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Extensions
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