using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class Division : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Number;

        internal override bool ResultInTempContainer => true;

        public Division(Expression first, Expression second)
            : base(first, second, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            int itemp;
            var jstemp = _left.Evaluate(context);
            if (jstemp._valueType == JSValueType.Integer
                || jstemp._valueType == JSValueType.Boolean)
            {
                itemp = jstemp._iValue;
                jstemp = _right.Evaluate(context);
                if ((jstemp._valueType == JSValueType.Boolean
                    || jstemp._valueType == JSValueType.Integer)
                    && jstemp._iValue > 0
                    && itemp > 0
                    && (itemp % jstemp._iValue) == 0)
                {
                    _tempContainer._valueType = JSValueType.Integer;
                    _tempContainer._iValue = itemp / jstemp._iValue;
                }
                else
                {
                    _tempContainer._valueType = JSValueType.Double;
                    _tempContainer._dValue = itemp / Tools.JSObjectToDouble(jstemp);
                }
                return _tempContainer;
            }
            _tempContainer._dValue = Tools.JSObjectToDouble(jstemp) / Tools.JSObjectToDouble(_right.Evaluate(context));
            _tempContainer._valueType = JSValueType.Double;
            return _tempContainer;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " / " + _right + ")";
        }
    }
}