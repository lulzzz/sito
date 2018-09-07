using System;

namespace JS.Core.Core.Interop
{
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
