using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ConvertToNumber : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Number;

        internal override bool ResultInTempContainer => true;

        public ConvertToNumber(Expression first)
            : base(first, null, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            return Tools.JSObjectToNumber(_left.Evaluate(context), _tempContainer);
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "+" + _left;
        }
    }
}