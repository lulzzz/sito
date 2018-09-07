
#define TYPE_SAFE

using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
    [Serializable]
    public sealed class Substract : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Number;

        internal override bool ResultInTempContainer => true;

        public Substract(Expression first, Expression second)
            : base(first, second, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            double da;
            var f = _left.Evaluate(context);
            JSValue s;
            if (f._valueType == JSValueType.Integer
                || f._valueType == JSValueType.Boolean)
            {
                var a = f._iValue;
                s = _right.Evaluate(context);
                if (s._valueType == JSValueType.Integer || s._valueType == JSValueType.Boolean)
                {
                    var l = (long) a - s._iValue;
                    //if (l > 2147483647L
                    //    || l < -2147483648L)
                    if (l != (int) l)
                    {
                        _tempContainer._dValue = l;
                        _tempContainer._valueType = JSValueType.Double;
                    }
                    else
                    {
                        _tempContainer._iValue = (int) l;
                        _tempContainer._valueType = JSValueType.Integer;
                    }

                    return _tempContainer;
                }

                da = a;
            }
            else
            {
                da = Tools.JSObjectToDouble(f);
                s = _right.Evaluate(context);
            }

            _tempContainer._dValue = da - Tools.JSObjectToDouble(s);
            _tempContainer._valueType = JSValueType.Double;
            return _tempContainer;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            var res = base.Build(ref _this, expressionDepth,  variables, codeContext, message, stats, opts);
            if (!res)
            {
                if (_left is Constant && Tools.JSObjectToDouble(_left.Evaluate(null)) == 0.0)
                {
                    _this = new Negation(_right);
                    return true;
                }
            }
            return res;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " - " + _right + ")";
        }
    }
}