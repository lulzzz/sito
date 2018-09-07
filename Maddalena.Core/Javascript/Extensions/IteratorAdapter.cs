using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Extensions
{
    internal sealed class IteratorAdapter : IterableProtocolBase, IIterator, IIterable
    {
        private readonly JSValue _iterator;

        [Hidden]
        public IteratorAdapter(JSValue iterator)
        {
            _iterator = iterator;
        }

        public IIteratorResult next(Arguments arguments = null)
        {
            var result = _iterator["next"].As<Function>().Call(_iterator, arguments);
            return new IteratorItemAdapter(result);
        }

        public IIteratorResult @return()
        {
            var result = _iterator["return"].As<Function>().Call(_iterator, null);
            return new IteratorItemAdapter(result);
        }

        public IIteratorResult @throw(Arguments arguments = null)
        {
            var result = _iterator["throw"].As<Function>().Call(_iterator, null);
            return new IteratorItemAdapter(result);
        }

        IIterator IIterable.iterator()
        {
            return this;
        }
    }
}