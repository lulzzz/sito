using System.Collections.Generic;
using JS.Core.Core;
using NiL.JS;

namespace JS.Core.Expressions
{
    public sealed class Super : Expression
    {
        public bool ctorMode;

        protected internal override bool ContextIndependent => false;

        protected internal override bool NeedDecompose => false;

        protected internal override bool LValueModifier => false;

        internal Super()
        {

        }

        protected internal override JSValue EvaluateForWrite(Context context)
        {
            ExceptionHelper.ThrowReferenceError(Messages.InvalidLefthandSideInAssignment);
            return null;
        }

        public override JSValue Evaluate(Context context)
        {
            if (ctorMode)
            {
                context._objectSource = context._thisBind;
                return context._owner.__proto__;
            }

            return context._thisBind;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {

        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "super";
        }
    }
}
