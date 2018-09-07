using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    /// <summary>
    /// Служит для передачи в среду выполнения скрипта информации о количестве ожидаемых параметров метода.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
    internal sealed class ArgumentsCountAttribute : Attribute
    {
        public int Count { get; }

        public ArgumentsCountAttribute(int count)
        {
            Count = count;
        }
    }
}
