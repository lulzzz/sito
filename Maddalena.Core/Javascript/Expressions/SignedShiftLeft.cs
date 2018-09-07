﻿using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class SignedShiftLeft : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Int;

        internal override bool ResultInTempContainer => true;

        public SignedShiftLeft(Expression first, Expression second)
            : base(first, second, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            _tempContainer._iValue = Tools.JSObjectToInt32(_left.Evaluate(context)) << Tools.JSObjectToInt32(_right.Evaluate(context));
            _tempContainer._valueType = JSValueType.Integer;
            return _tempContainer;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            var res = base.Build(ref _this, expressionDepth,  variables, codeContext, message, stats, opts);
            if (!res && _this == this)
            {
                try
                {
                    if (_left.ContextIndependent && Tools.JSObjectToInt32((_left).Evaluate(null)) == 0)
                        _this = new Constant(0);
                    else if (_right.ContextIndependent && Tools.JSObjectToInt32((_right).Evaluate(null)) == 0)
                        _this = new ConvertToInteger(_left);
                }
                catch
                {

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
            return "(" + _left + " << " + _right + ")";
        }
    }
}