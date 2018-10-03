using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    /// <summary>
    /// Указывает, какой тип необходимо представить в цепочке прототипов объекта-прослойки для помеченного типа.
    /// </summary>
#if !(PORTABLE)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public sealed class PrototypeAttribute : Attribute
    {
        public Type PrototypeType { get; }
        public bool Replace { get; }

        public PrototypeAttribute(Type type)
            : this(type, false)
        {
        }

        internal PrototypeAttribute(Type type, bool doNotChainButReplace)
        {
            Replace = doNotChainButReplace;
            PrototypeType = type;
        }
    }
}
