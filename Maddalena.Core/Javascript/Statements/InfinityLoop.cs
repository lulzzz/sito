using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Expressions;

namespace Maddalena.Core.Javascript.Statements
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class InfinityLoop : CodeNode
    {
        private CodeNode body;
        private readonly string[] labels;

        public CodeNode Body => body;
        public ReadOnlyCollection<string> Labels => new ReadOnlyCollection<string>(labels);

        internal InfinityLoop(CodeNode body, string[] labels)
        {
            this.body = body ?? new Empty();
            this.labels = labels;
        }

        public override JSValue Evaluate(Context context)
        {
            for (;;)
            {
                if (context.Debugging && !(body is CodeBlock))
                    context.raiseDebugger(body);

                context._lastResult = body.Evaluate(context) ?? context._lastResult;
                if (context._executionMode != ExecutionMode.None)
                {
                    if (context._executionMode < ExecutionMode.Return)
                    {
                        var me = context._executionInfo == null || Array.IndexOf(labels, context._executionInfo._oValue as string) != -1;
                        var _break = (context._executionMode > ExecutionMode.Continue) || !me;
                        if (me)
                        {
                            context._executionMode = ExecutionMode.None;
                            context._executionInfo = JSValue.notExists;
                        }
                        if (_break)
                            return null;
                    }
                    else
                        return null;
                }
            }
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return new[] { body };
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            body.Optimize(ref body, owner, message, opts, stats);
        }

        public override void Decompose(ref CodeNode self)
        {
            body.Decompose(ref body);
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            body.RebuildScope(functionInfo, transferedVariables, scopeBias);
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "for (;;)" + (body is CodeBlock ? "" : Environment.NewLine + "  ") + body;
        }
    }
}