using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.JIT;

namespace Maddalena.Core.Javascript.Expressions
{
    [Serializable]
    public sealed class NumberAddition : Expression
    {
        protected internal override PredictedType ResultType
        {
            get
            {
                var pd = _left.ResultType;
                switch (pd)
                {
                    case PredictedType.Double:
                        {
                            return PredictedType.Double;
                        }
                    default:
                        {
                            return PredictedType.Number;
                        }
                }
            }
        }

        internal override bool ResultInTempContainer => true;

        public NumberAddition(Expression first, Expression second)
            : base(first, second, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var op = _left.Evaluate(context);
            switch (op._valueType)
            {
                case JSValueType.Integer:
                    var itemp = op._iValue;
                    op = _right.Evaluate(context);
                    switch (op._valueType)
                    {
                        case JSValueType.Integer:
                            var ltemp = (long)itemp + op._iValue;
                            if ((int)ltemp == ltemp)
                            {
                                _tempContainer._valueType = JSValueType.Integer;
                                _tempContainer._iValue = (int)ltemp;
                            }
                            else
                            {
                                _tempContainer._valueType = JSValueType.Double;
                                _tempContainer._dValue = ltemp;
                            }

                            break;
                        case JSValueType.Double:
                            _tempContainer._valueType = JSValueType.Double;
                            _tempContainer._dValue = itemp + op._dValue;
                            break;
                        default:
                            _tempContainer._valueType = JSValueType.Integer;
                            _tempContainer._iValue = itemp;
                            Addition.Impl(_tempContainer, _tempContainer, op);
                            break;
                    }

                    break;
                case JSValueType.Double:
                    var dtemp = op._dValue;
                    op = _right.Evaluate(context);
                    switch (op._valueType)
                    {
                        case JSValueType.Integer:
                            _tempContainer._valueType = JSValueType.Double;
                            _tempContainer._dValue = dtemp + op._iValue;
                            break;
                        case JSValueType.Double:
                            _tempContainer._valueType = JSValueType.Double;
                            _tempContainer._dValue = dtemp + op._dValue;
                            break;
                        default:
                            _tempContainer._valueType = JSValueType.Double;
                            _tempContainer._dValue = dtemp;
                            Addition.Impl(_tempContainer, _tempContainer, op);
                            break;
                    }

                    break;
                default:
                    Addition.Impl(_tempContainer, op.CloneImpl(false), _right.Evaluate(context));
                    break;
            }
            return _tempContainer;
        }

        internal override System.Linq.Expressions.Expression TryCompile(bool selfCompile, bool forAssign, Type expectedType, List<CodeNode> dynamicValues)
        {
            var ft = _left.TryCompile(false, false, null, dynamicValues);
            var st = _right.TryCompile(false, false, null, dynamicValues);
            if (ft == st) // null == null
                return null;
            if (ft == null && st != null)
            {
                _right = new CompiledNode(_right, st, JITHelpers._items.GetValue(dynamicValues) as CodeNode[]);
                return null;
            }
            if (ft != null && st == null)
            {
                _left = new CompiledNode(_left, ft, JITHelpers._items.GetValue(dynamicValues) as CodeNode[]);
                return null;
            }
            if (ft.Type == st.Type && (ft.Type == typeof(double) || ft.Type == expectedType))
                return System.Linq.Expressions.Expression.Add(ft, st);
            return System.Linq.Expressions.Expression.Add(
                System.Linq.Expressions.Expression.Convert(ft, typeof(double)),
                System.Linq.Expressions.Expression.Convert(st, typeof(double)));
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " + " + _right + ")";
        }
    }
}
