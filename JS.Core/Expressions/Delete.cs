using System;
using System.Collections.Generic;
using JS.Core.Core;
using NiL.JS;
using NiL.JS.BaseLibrary;

namespace JS.Core.Expressions
{
    [Serializable]
    public sealed class Delete : Expression
    {
        protected internal override PredictedType ResultType => PredictedType.Bool;

        protected internal override bool ContextIndependent => false;

        internal override bool ResultInTempContainer => false;

        public Delete(Expression first)
            : base(first, null, false)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var temp = _left.Evaluate(context);
            if (temp._valueType < JSValueType.Undefined)
                return true;

            else if ((temp._attributes & JSValueAttributesInternal.Argument) != 0)
            {
                return false;
            }
            else if ((temp._attributes & JSValueAttributesInternal.DoNotDelete) == 0)
            {
                if ((temp._attributes & JSValueAttributesInternal.SystemObject) == 0)
                {
                    temp._valueType = JSValueType.NotExists;
                    temp._oValue = null;
                }

                return true;
            }
            else if (context._strict)
            {
                ExceptionHelper.Throw(new TypeError("Can not delete property \"" + _left + "\"."));
            }

            return false;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts))
                return true;
            if (_left is Variable)
            {
                if ((codeContext & CodeContext.Strict) != 0)
                    ExceptionHelper.Throw(new SyntaxError("Can not delete variable in strict mode"));
                ((Variable) _left)._SuspendThrow = true;
            }

            if (_left is Property gme)
            {
                _this = new DeleteProperty(gme._left, gme._right);
                return false;
            }
            var f = _left as VariableReference ?? ((_left is AssignmentOperatorCache) ? (_left as AssignmentOperatorCache).Source as VariableReference : null);
            if (f != null)
            {
                if (f.Descriptor.IsDefined && message != null)
                    message(MessageLevel.Warning, Position, Length, "Tring to delete defined variable." + ((codeContext & CodeContext.Strict) != 0 ? " In strict mode it cause exception." : " It is not allowed"));
                (f.Descriptor.assignments ??
                    (f.Descriptor.assignments = new System.Collections.Generic.List<Expression>())).Add(this);
            }
            return false;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "delete " + _left;
        }
    }
}