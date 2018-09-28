using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    [Serializable]
    [Prototype(typeof(JSObject), true)]
    internal sealed class PrototypeProxy : Proxy
    {
        internal override bool IsInstancePrototype => true;

        public PrototypeProxy(GlobalContext context, Type type, bool indexersSupport)
            : base(context, type, indexersSupport)
        {
        }
    }
}
