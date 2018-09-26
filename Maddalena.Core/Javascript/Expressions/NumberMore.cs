using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
    [Serializable]
    public sealed class NumberMore : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        internal override bool ResultInTempContainer => false;

        public NumberMore(Expression first, Expression second)
            : base(first, second, false)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var op = _left.Evaluate(context);
            switch (op._valueType)
            {
                case JSValueType.Integer:
                case JSValueType.Boolean:
                    var itemp = op._iValue;
                    op = _right.Evaluate(context);
                    switch (op._valueType)
                    {
                        case JSValueType.Integer:
                        case JSValueType.Boolean:
                            return itemp > op._iValue;
                        case JSValueType.Double:
                            return itemp > op._dValue;
                    }

                    if (_tempContainer == null)
                        _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };
                    _tempContainer._valueType = JSValueType.Integer;
                    _tempContainer._iValue = itemp;
                    return More.Check(_tempContainer, op, false);

                case JSValueType.Double:
                    var dtemp = op._dValue;
                    op = _right.Evaluate(context);
                    switch (op._valueType)
                    {
                        case JSValueType.Integer:
                        case JSValueType.Boolean:
                            return dtemp > op._iValue;
                        case JSValueType.Double:
                            return dtemp > op._dValue;
                    }

                    if (_tempContainer == null)
                        _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };
                    _tempContainer._valueType = JSValueType.Double;
                    _tempContainer._dValue = dtemp;
                    return More.Check(_tempContainer, op, false);
            }

            if (_tempContainer == null)
                _tempContainer = new JSValue { _attributes = JsValueAttributesInternal.Temporary };

            var temp = _tempContainer;
            temp.Assign(op);
            _tempContainer = null;
            var res = More.Check(temp, _right.Evaluate(context), false);
            _tempContainer = temp;
            return res;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"({_left} > {_right})";
        }
    }
}
