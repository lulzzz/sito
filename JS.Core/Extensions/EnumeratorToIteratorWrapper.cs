using System.Collections;
using JS.Core.Core;
using JS.Core.Core.Interop;

namespace JS.Core.Extensions
{
    internal sealed class EnumeratorToIteratorWrapper : IterableProtocolBase, IIterator, IIterable
    {
        private readonly IEnumerator _enumerator;
        private readonly GlobalContext _context;

        [Hidden]
        public EnumeratorToIteratorWrapper(IEnumerator enumerator)
        {
            _enumerator = enumerator;
            _context = Context.CurrentGlobalContext;
        }

        public IIterator iterator()
        {
            return this;
        }

        public IIteratorResult next(Arguments arguments = null)
        {
            var read = _enumerator.MoveNext();
            return new EnumeratorResult(
                !read,
                _context.ProxyValue(read ? _enumerator.Current : null));
        }

        public IIteratorResult @return()
        {
            return new EnumeratorResult(true, null);
        }

        public IIteratorResult @throw(Arguments arguments = null)
        {
            return new EnumeratorResult(true, null);
        }
    }
}