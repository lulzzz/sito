using System;

namespace JS.Core.Core.Interop
{
    [Prototype(typeof(JSObject), true)]
    internal sealed class StaticProxy : Proxy
    {
        internal override JSObject PrototypeInstance => null;

        internal override bool IsInstancePrototype => false;

        [Hidden]
        public StaticProxy(GlobalContext context, Type type, bool indexersSupport)
            : base(context, type, indexersSupport)
        {

        }
    }
}
