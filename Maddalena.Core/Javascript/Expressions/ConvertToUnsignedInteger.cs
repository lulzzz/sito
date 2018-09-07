﻿using System;
using JS.Core.Core;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ConvertToUnsignedInteger : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Number;

        internal override bool ResultInTempContainer => true;

        public ConvertToUnsignedInteger(Expression first)
            : base(first, null, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var t = (uint)Tools.JSObjectToInt32(_left.Evaluate(context));
            if (t <= int.MaxValue)
            {
                _tempContainer._iValue = (int)t;
                _tempContainer._valueType = JSValueType.Integer;
            }
            else
            {
                _tempContainer._dValue = t;
                _tempContainer._valueType = JSValueType.Double;
            }
            return _tempContainer;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " | 0)";
        }
    }
}