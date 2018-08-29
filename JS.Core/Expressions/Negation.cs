using System;
using JS.Core.Core;

namespace JS.Core.Expressions
{
    [Serializable]
    public sealed class Negation : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Number;

        internal override bool ResultInTempContainer => true;

        public Negation(Expression first)
            : base(first, null, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var val = _left.Evaluate(context);
            if (val._valueType == JSValueType.Integer
                || val.ValueType == JSValueType.Boolean)
            {
                switch (val._iValue)
                {
                    case 0:
                        _tempContainer._valueType = JSValueType.Double;
                        _tempContainer._dValue = -0.0;
                        break;
                    case int.MinValue:
                        _tempContainer._valueType = JSValueType.Double;
                        _tempContainer._dValue = val._iValue;
                        break;
                    default:
                        _tempContainer._valueType = JSValueType.Integer;
                        _tempContainer._iValue = -val._iValue;
                        break;
                }
            }
            else
            {
                _tempContainer._dValue = -Tools.JSObjectToDouble(val);
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
            return "-" + _left;
        }
    }
}