using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class BitwiseConjunction : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Int;

        internal override bool ResultInTempContainer => true;

        public BitwiseConjunction(Expression first, Expression second)
            : base(first, second, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            _tempContainer._iValue = Tools.JSObjectToInt32(_left.Evaluate(context)) & Tools.JSObjectToInt32(_right.Evaluate(context));
            _tempContainer._valueType = JSValueType.Integer;
            return _tempContainer;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " & " + _right + ")";
        }
    }
}