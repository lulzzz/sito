using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ConvertToString : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.String;

        internal override bool ResultInTempContainer => true;

        public ConvertToString(Expression first)
            : base(first, null, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var t = _left.Evaluate(context);
            if (t._valueType == JSValueType.String)
                return t;
            _tempContainer._valueType = JSValueType.String;
            _tempContainer._oValue = t.ToPrimitiveValue_Value_String().ToString();
            return _tempContainer;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "( \"\" + " + _left + ")";
        }
    }
}