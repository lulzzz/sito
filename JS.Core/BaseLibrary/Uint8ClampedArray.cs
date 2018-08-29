﻿using System;
using JS.Core.Core;
using JS.Core.Core.Interop;

namespace NiL.JS.BaseLibrary
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class Uint8ClampedArray : TypedArray
    {
        protected override JSValue this[int index]
        {
            get
            {
                var res = new Element(this, index);
                res._iValue = getValue(index);
                res._valueType = JSValueType.Integer;
                return res;
            }
            set
            {
                if (index < 0 || index > length._iValue)
                    ExceptionHelper.Throw(new RangeError());
                buffer.data[index + byteOffset] = (byte)System.Math.Min(255, System.Math.Max(0, Tools.JSObjectToInt32(value, 0, false)));
            }
        }

        private byte getValue(int index)
        {
            return buffer.data[index + byteOffset];
        }

        public override int BYTES_PER_ELEMENT => sizeof(byte);

        public Uint8ClampedArray()
            : base() { }

        public Uint8ClampedArray(int length)
            : base(length) { }

        public Uint8ClampedArray(ArrayBuffer buffer)
            : base(buffer, 0, buffer.byteLength) { }

        public Uint8ClampedArray(ArrayBuffer buffer, int bytesOffset)
            : base(buffer, bytesOffset, buffer.byteLength - bytesOffset) { }

        public Uint8ClampedArray(ArrayBuffer buffer, int bytesOffset, int length)
            : base(buffer, bytesOffset, length) { }

        public Uint8ClampedArray(JSValue src)
            : base(src) { }

        [ArgumentsCount(2)]
        public override TypedArray subarray(Arguments args)
        {
            return subarrayImpl<Uint8ClampedArray>(args[0], args[1]);
        }

        [Hidden]
        public override Type ElementType
        {
            [Hidden]
            get { return typeof(byte); }
        }

        protected internal override System.Array ToNativeArray()
        {
            var res = new byte[length._iValue];
            for (var i = 0; i < res.Length; i++)
                res[i] = getValue(i);
            return res;
        }
    }
}
