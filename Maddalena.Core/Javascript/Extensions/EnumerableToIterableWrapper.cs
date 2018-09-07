using System.Collections;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Extensions
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