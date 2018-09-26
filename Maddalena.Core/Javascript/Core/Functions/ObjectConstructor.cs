using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Core.Functions
{
#if !(PORTABLE)
    [Serializable]
#endif
    [Prototype(typeof(Function), true)]
    internal class ObjectConstructor : ConstructorProxy
    {
        public override string name => "Object";

        public ObjectConstructor(Context context, StaticProxy staticProxy, JSObject prototype)
            : base(context, staticProxy, prototype)
        {
            _length = new Number(1);
        }

        protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
        {
            JSValue nestedValue = targetObject;
            if (nestedValue != null && (nestedValue._attributes & JsValueAttributesInternal.ConstructingObject) == 0)
                nestedValue = null;

            if (arguments != null && arguments.length > 0)
                nestedValue = arguments[0];

            if (nestedValue == null)
                return ConstructObject();

            if (nestedValue._valueType >= JSValueType.Object)
            {
                if (nestedValue._oValue == null)
                    return ConstructObject();

                return nestedValue;
            }

            if (nestedValue._valueType <= JSValueType.Undefined)
                return ConstructObject();

            return nestedValue.ToObject();
        }

        protected internal override JSValue ConstructObject()
        {
            return CreateObject();
        }

        protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool hideNonEnum, EnumerationMode enumerationMode)
        {
            var pe = _staticProxy.GetEnumerator(hideNonEnum, enumerationMode);
            while (pe.MoveNext())
                yield return pe.Current;
            pe = __proto__.GetEnumerator(hideNonEnum, enumerationMode);
            while (pe.MoveNext())
                yield return pe.Current;
        }
    }
}
