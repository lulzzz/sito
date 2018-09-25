﻿using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Statements
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class Continue : CodeNode
    {
        public JSValue Label { get; private set; }

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            int i = index;
            if (!Parser.Validate(state.Code, "continue", ref i) || !Parser.IsIdentifierTerminator(state.Code[i]))
                return null;
            if (!state.AllowContinue.Peek())
                ExceptionHelper.Throw((new SyntaxError("Invalid use of continue statement")));
            while (Tools.IsWhiteSpace(state.Code[i]) && !Tools.IsLineTerminator(state.Code[i])) i++;
            int sl = i;
            JSValue label = null;
            if (Parser.ValidateName(state.Code, ref i, state.Strict))
            {
                label = Tools.Unescape(state.Code.Substring(sl, i - sl), state.Strict);
                if (!state.Labels.Contains(label._oValue.ToString()))
                    ExceptionHelper.Throw((new SyntaxError("Try to continue to undefined label.")));
            }
            int pos = index;
            index = i;
            state.ContiniesCount++;
            return new Continue
            {
                Label = label,
                Position = pos,
                Length = index - pos
            };
        }

        public override JSValue Evaluate(Context context)
        {
            context._executionMode = ExecutionMode.Continue;
            context._executionInfo = Label;
            return null;
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return null;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Decompose(ref CodeNode self)
        {

        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {

        }

        public override string ToString()
        {
            return "continue" + (Label != null ? " " + Label : "");
        }
    }
}