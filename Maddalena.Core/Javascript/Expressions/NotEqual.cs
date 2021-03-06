﻿using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class NotEqual : Equal
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        public NotEqual(Expression first, Expression second)
            : base(first, second)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            return base.Evaluate(context)._iValue == 0;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " != " + _right + ")";
        }
    }
}