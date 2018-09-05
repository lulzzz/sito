﻿using System;
using System.Collections.Generic;
using JS.Core.Core.Interop;

namespace JS.Core.Core
{
#if !(PORTABLE)
    [Serializable]
#endif
    [Prototype(typeof(JSObject), true)]
    internal sealed class GlobalObject : JSObject
    {
        private readonly Context _context;

        public GlobalObject(Context context)
        {
            _attributes = JSValueAttributesInternal.SystemObject;
            _context = context;
            _fields = context._variables;
            _valueType = JSValueType.Object;
            _oValue = this;
            _objectPrototype = context.GlobalContext._globalPrototype;
        }

        protected internal override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope memberScope)
        {
            if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
            {
                var nameStr = key.ToString();
                var res = _context.GetVariable(nameStr, forWrite);
                return res;
            }

            return base.GetProperty(key, forWrite, memberScope);
        }

        protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool hideNonEnumerable, EnumerationMode enumerationMode)
        {
            foreach (var i in _context._variables)
                if (i.Value.Exists && (!hideNonEnumerable || (i.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == 0))
                    yield return i;

            foreach (var i in _context.GlobalContext._variables)
                if (i.Value.Exists && (!hideNonEnumerable || (i.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == 0))
                    yield return i;
        }

        public override string ToString()
        {
            return "[object global]";
        }
    }
}