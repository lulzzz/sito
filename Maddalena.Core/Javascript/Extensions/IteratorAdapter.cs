using JS.Core.Core;
using JS.Core.Core.Interop;
using NiL.JS.BaseLibrary;

namespace JS.Core.Extensions
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