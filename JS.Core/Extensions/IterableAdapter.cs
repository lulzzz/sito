using JS.Core.Core;
using JS.Core.Core.Interop;
using NiL.JS.BaseLibrary;

namespace JS.Core.Extensions
{
    internal sealed class IterableAdapter : IterableProtocolBase, IIterable
    {
        private readonly JSValue _source;

        [Hidden]
        public IterableAdapter(JSValue source)
        {
            this._source = source.IsBox ? source._oValue as JSValue : source;
        }

        public IIterator iterator()
        {
            var iteratorFunction = _source.GetProperty(Symbol.iterator, false, PropertyScope.Common);
            if (iteratorFunction._valueType != JSValueType.Function)
                return null;

            var iterator = iteratorFunction.As<Function>().Call(_source, null);
            if (iterator == null)
                return null;

            return new IteratorAdapter(iterator);
        }
    }
}