using System.Collections;
using JS.Core.Core;
using JS.Core.Core.Interop;

namespace JS.Core.Extensions
{
    internal sealed class EnumerableToIterableWrapper : IterableProtocolBase, IIterable
    {
        private readonly IEnumerable enumerable;

        [Hidden]
        public EnumerableToIterableWrapper(IEnumerable enumerable)
        {
            this.enumerable = enumerable;
        }

        public IIterator iterator()
        {
            return new EnumeratorToIteratorWrapper(enumerable.GetEnumerator());
        }
    }
}