﻿using System;
using System.Collections.Generic;
using JS.Core.Core;
using JS.Core.Expressions;

namespace NiL.JS.Statements
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class LabeledStatement : CodeNode
    {
        private CodeNode statement;

        public CodeNode Statement => statement;
        public string Label { get; private set; }

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            int i = index;
            if (!Parser.ValidateName(state.Code, ref i, state.strict))
                return null;
            int l = i;
            if (i >= state.Code.Length || (!Parser.Validate(state.Code, " :", ref i) && state.Code[i++] != ':'))
                return null;

            var label = state.Code.Substring(index, l - index);
            state.Labels.Add(label);
            int oldlc = state.LabelsCount;
            state.LabelsCount++;
            var stat = Parser.Parse(state, ref i, 0);
            state.Labels.Remove(label);
            state.LabelsCount = oldlc;
            if (stat is FunctionDefinition)
            {
                state.message?.Invoke(MessageLevel.CriticalWarning, stat.Position, stat.Length, "Labeled function. Are you sure?");
            }
            var pos = index;
            index = i;
            return new LabeledStatement
            {
                statement = stat,
                Label = label,
                Position = pos,
                Length = index - pos
            };
        }

        public override JSValue Evaluate(Context context)
        {
            var res = statement.Evaluate(context);
            if ((context._executionMode == ExecutionMode.Break) && (context._executionInfo != null) && (context._executionInfo._oValue as string == Label))
            {
                context._executionMode = ExecutionMode.None;
                context._executionInfo = JSValue.notExists;
            }
            return res;
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return new[] { statement };
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            Parser.Build(ref statement, expressionDepth, variables, codeContext, message, stats, opts);
            if (statement == null)
            {
                _this = null;
            }

            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            statement.Optimize(ref statement, owner, message, opts, stats);
            if (statement == null)
            {
                _this = null;
            }
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return Label + ": " + statement;
        }

        public override void Decompose(ref CodeNode self)
        {
            statement.Decompose(ref statement);
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            statement.RebuildScope(functionInfo, transferedVariables, scopeBias);
        }
    }
}