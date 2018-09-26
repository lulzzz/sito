using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class NumberLess : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        internal override bool ResultInTempContainer => false;

        public NumberLess(Expression first, Expression second)
            : base(first, second, false)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            int itemp;
            double dtemp;
            var op = _left.Evaluate(context);
            if (op._valueType == JSValueType.Integer)
            {
                itemp = op._iValue;
                op = _right.Evaluate(context);
                if (op._valueType == JSValueType.Integer)
                {
                    return itemp < op._iValue;
                }

                if (op._valueType == JSValueType.Double)
                {
                    return itemp < op._dValue;
                }

                if (_tempContainer == null)
                    _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };
                _tempContainer._valueType = JSValueType.Integer;
                _tempContainer._iValue = itemp;
                return Less.Check(_tempContainer, op);
            }

            if (op._valueType == JSValueType.Double)
            {
                dtemp = op._dValue;
                op = _right.Evaluate(context);
                if (op._valueType == JSValueType.Integer)
                {
                    return dtemp < op._iValue;
                }

                if (op._valueType == JSValueType.Double)
                {
                    return dtemp < op._dValue;
                }

                if (_tempContainer == null)
                    _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };
                _tempContainer._valueType = JSValueType.Double;
                _tempContainer._dValue = dtemp;
                return Less.Check(_tempContainer, op);
            }

            if (_tempContainer == null)
                _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };
            var temp = _tempContainer;
            temp.Assign(op);
            _tempContainer = null;
            var res = Less.Check(temp, _right.Evaluate(context));
            _tempContainer = temp;
            return res;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " < " + _right + ")";
        }
    }
}
