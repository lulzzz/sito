using System;
using System.Collections.Generic;

namespace Maddalena.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Random<T>(T[] enu, int size)
        {
            var rand = new Random();

            for (int i = 0; i < size; i++)
            {
                yield return enu[rand.Next(enu.Length - 1)];
            }
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            List<T> nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch.ToArray();
                    nextbatch = new List<T>(batchSize);
                }
            }
            if (nextbatch.Count > 0)
                yield return nextbatch.ToArray();
        }
    }
}
