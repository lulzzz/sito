using System;
using System.Collections.Generic;
using System.Reflection;
using JS.Core.Core.JIT;
using JS.Core.Expressions;
using NiL.JS;

#if !(PORTABLE)

#endif

namespace JS.Core.Core
{
    [Flags]
    public enum CodeContext
    {
        None = 0,
        Strict = 1,
        //ForAssign = 2,
        Conditional = 4,
        InLoop = 8,
        InWith = 16,
        InEval = 32,
        InExpression = 64,
        InClassDefenition = 128,
        InClassConstructor = 256,
        InStaticMember = 512,
        InGenerator = 1024,
        InFunction = 2048,
        InAsync = 4096
    }

#if !(PORTABLE)
    [Serializable]
#endif
    public abstract class CodeNode
    {
        private static readonly CodeNode[] emptyCodeNodeArray = new CodeNode[0];

#if !NET35 && !(PORTABLE)
        internal System.Linq.Expressions.Expression JitOverCall(bool forAssign)
        {
            return System.Linq.Expressions.Expression.Call(
                System.Linq.Expressions.Expression.Constant(this),
                GetType().GetMethod(forAssign ? "EvaluateForWrite" : "Evaluate", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Context) }, null),
                JITHelpers.ContextParameter
                );
        }
#endif

        public virtual bool Eliminated { get; internal set; }
        public virtual int Position { get; internal set; }
        public virtual int Length { get; internal set; }
        public int EndPosition => Position + Length;

        private CodeNode[] childs;
        public CodeNode[] Childs => childs ?? (childs = GetChildsImpl() ?? emptyCodeNodeArray);

        protected internal virtual CodeNode[] GetChildsImpl()
        {
            return new CodeNode[0];
        }

        protected internal virtual JSValue EvaluateForWrite(Context context)
        {
            ExceptionHelper.ThrowReferenceError(Messages.InvalidLefthandSideInAssignment);
            return null;
        }

        public abstract JSValue Evaluate(Context context);

        public virtual bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            return false;
        }

        public virtual void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {

        }
#if !PORTABLE
        internal virtual System.Linq.Expressions.Expression TryCompile(bool selfCompile, bool forAssign, Type expectedType, List<CodeNode> dynamicValues)
        {
            return null;
        }
#endif

        public abstract void Decompose(ref CodeNode self);

        public abstract void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias);

        public virtual T Visit<T>(Visitor<T> visitor)
        {
            return default(T);
        }
    }
}
