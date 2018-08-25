using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maddalena.Datastorage
{
    static class Extensions
    {
        public static IEnumerable<T> Wait<T>(this IEnumerable<Task<T>> tasks)
        {
            foreach (var item in tasks)
            {
                item.Wait();
                yield return item.Result;
            }
        }
    }
}
