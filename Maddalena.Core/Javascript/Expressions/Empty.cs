using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class Empty : Expression
    {
        public static Empty Instance { get; } = new Empty();

        internal override bool ResultInTempContainer => false;

        protected internal override PredictedType ResultType => PredictedType.Undefined;

        public Empty()
            : base(null, null, false)
        {
        }

        public Empty(int position)
            : base(null, null, false)
        {
            Position = position;
            Length = 0;
        }

        public override JSValue Evaluate(Context context)
        {
            return null;
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return null;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (expressionDepth < 2)
            {
                _this = null;
                Eliminated = true;
            }
            return false;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "";
        }
    }
}