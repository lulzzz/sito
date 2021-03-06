﻿using System;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Array = Maddalena.Core.Javascript.BaseLibrary.Array;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class In : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        internal override bool ResultInTempContainer => false;

        public In(Expression first, Expression second)
            : base(first, second, false)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            bool res;
            if (_tempContainer == null)
                _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };
            _tempContainer.Assign(_left.Evaluate(context));
            var temp = _tempContainer;
            _tempContainer = null;
            var source = _right.Evaluate(context);
            if (source._valueType < JSValueType.Object)
                ExceptionHelper.Throw(new TypeError("Right-hand value of operator in is not object."));
            if (temp._valueType == JSValueType.Integer)
            {
                var array = source._oValue as Array;
                if (array != null)
                {
                    res = temp._iValue >= 0 && temp._iValue < array._data.Length && (array._data[temp._iValue] ?? JSValue.notExists).Exists;
                    _tempContainer = temp;
                    return res;
                }
            }
            var t = source.GetProperty(temp, false, PropertyScope.Common);
            _tempContainer = temp;
            return t.Exists;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " in " + _right + ")";
        }
    }
}