using System;
using JS.Core.Core;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ConvertToBoolean : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        internal override bool ResultInTempContainer => false;

        public ConvertToBoolean(Expression first)
            : base(first, null, false)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            return (bool)_left.Evaluate(context);
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "!!" + _left;
        }
    }
}
