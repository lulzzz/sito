using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Extensions
{
    internal sealed class IterableAdapter : IterableProtocolBase, IIterable
    {
        private readonly JSValue _source;

        [Hidden]
        public IterableAdapter(JSValue source)
        {
            _source = source.IsBox ? source._oValue as JSValue : source;
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