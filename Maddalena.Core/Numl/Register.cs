using System.Reflection;
using Maddalena.Core.Numl.Utils;

namespace Maddalena.Core.Numl
{
    public static class Register
    {
        /// <summary>
        /// Registration for Maddalena.Numl to understand all of
        /// your types
        /// </summary>
        /// <param name="assemblies">The assembly.</param>
        public static void Assembly(params Assembly[] assemblies)
        {
            // register assemblies
            foreach (var a in assemblies)
                Ject.AddAssembly(a);
        }

        public static void Type<T>()
        {
            Assembly(typeof(T).GetTypeInfo().Assembly);
        }
    }
}
