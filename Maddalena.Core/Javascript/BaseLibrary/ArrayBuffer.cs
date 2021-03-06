﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.BaseLibrary
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ArrayBuffer : CustomType
    {
#if !(PORTABLE)
        [Serializable]
#endif
        private sealed class Element : JSValue
        {
            private readonly int index;
            private readonly byte[] data;

            public Element(int index, ArrayBuffer parent)
            {
                _valueType = JSValueType.Integer;
                this.index = index;
                _iValue = parent.data[index];
                data = parent.data;
                _attributes |= JsValueAttributesInternal.Reassign;
            }

            public override void Assign(JSValue value)
            {
                data[index] = (byte)Tools.JSObjectToInt32(value);
            }
        }

        internal byte[] data;

        [Hidden]
        public byte this[int index]
        {
            [Hidden]
            get
            {
                return data[index];
            }
            [Hidden]
            set
            {
                data[index] = value;
            }
        }

        [DoNotEnumerate]
        public ArrayBuffer()
            : this(0)
        {
        }

        [DoNotEnumerate]
        public ArrayBuffer(int length)
            : this(new byte[length])
        {
        }

        [Hidden]
        public ArrayBuffer(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException();
            this.data = data;
            _attributes |= JsValueAttributesInternal.SystemObject;
        }

        public int byteLength
        {
            [Hidden]
            get
            {
                return data.Length;
            }
        }

        [Hidden]
        public ArrayBuffer slice(int begin, int end)
        {
            if (end < begin || begin >= data.Length || end >= data.Length)
                ExceptionHelper.Throw((new RangeError("Invalid begin or end index")));

            var res = new ArrayBuffer(end - begin + 1);
            for (int i = 0, j = begin; j <= end; j++, i++)
                res.data[i] = data[j];

            return res;
        }

        [Hidden]
        public ArrayBuffer slice(int begin)
        {
            return slice(begin, data.Length - 1);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ArrayBuffer slice(Arguments args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            var l = Tools.JSObjectToInt32(args.GetProperty("length"));
            if (l == 0)
                return this;
            if (l == 1)
                return slice(Tools.JSObjectToInt32(args[0]), data.Length - 1);
            return slice(Tools.JSObjectToInt32(args[0]), Tools.JSObjectToInt32(args[1]));
        }

        [Hidden]
        protected internal override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope memberScope)
        {
            if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
            {
                uint index = 0;
                double dindex = Tools.JSObjectToDouble(key);
                if (!double.IsInfinity(dindex)
                    && !double.IsNaN(dindex)
                    && ((index = (uint)dindex) == dindex))
                {
                    return getElement((int)index);
                }
            }
            return base.GetProperty(key, forWrite, memberScope);
        }

        private JSValue getElement(int index)
        {
            if (index < 0)
                ExceptionHelper.Throw(new RangeError("Invalid array index"));
            if (index >= data.Length)
                return undefined;
            return new Element(index, this);
        }

        protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool hideNonEnumerable, EnumerationMode enumerationMode)
        {
            var be = base.GetEnumerator(hideNonEnumerable, enumerationMode);
            while (be.MoveNext())
                yield return be.Current;

            for (var i = 0; i < data.Length; i++)
                yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), (int)enumerationMode > 0 ? getElement(i) : null);
        }

        [Hidden]
        public byte[] GetData()
        {
            return data;
        }
    }
}
