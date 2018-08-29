using System;
using System.Collections;
using System.Collections.Generic;
using JS.Core.Core;

namespace JS.Core.Extensions
{
    public static class IterationProtocolExtensions
    {
        public static IEnumerable<JSValue> AsEnumerable(this IIterable iterableObject)
        {
            var iterator = iterableObject.iterator();
            if (iterator == null)
                yield break;

            var item = iterator.next();
            while (!item.done)
            {
                yield return item.value;
                item = iterator.next();
            }
        }

        public static IIterable AsIterable(this JSValue source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Value as IIterable ?? new IterableAdapter(source);
        }

        public static IIterable AsIterable(this IEnumerable enumerable)
        {
            return new EnumerableToIterableWrapper(enumerable);
        }

        public static IIterator AsIterator(this IEnumerator enumerator)
        {
            return new EnumeratorToIteratorWrapper(enumerator);
        }
    }
}
