﻿using System;
using JS.Core.Core;
using NiL.JS;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class MoreOrEqual : Less
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        public MoreOrEqual(Expression first, Expression second)
            : base(first, second)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            return base.Evaluate(context)._iValue == 0;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            baseOptimize(ref _this, owner, message, opts, stats);
            if (_this == this)
                if (_left.ResultType == PredictedType.Number && _right.ResultType == PredictedType.Number)
                {
                    _this = new NumberMoreOrEqual(_left, _right);
                }
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + _left + " >= " + _right + ")";
        }
    }
}