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
    }
}
