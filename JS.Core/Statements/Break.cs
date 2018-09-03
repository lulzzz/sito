﻿using System;
using System.Collections.Generic;
using JS.Core.Core;
using NiL.JS.BaseLibrary;

namespace NiL.JS.Statements
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class Break : CodeNode
    {
        public JSValue Label { get; private set; }

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            int i = index;
            if (!Parser.Validate(state.Code, "break", ref i) || !Parser.IsIdentifierTerminator(state.Code[i]))
                return null;
            while (Tools.IsWhiteSpace(state.Code[i]) && !Tools.IsLineTerminator(state.Code[i]))
                i++;
            int sl = i;
            JSValue label = null;
            if (Parser.ValidateName(state.Code, ref i, state.strict))
            {
                label = Tools.Unescape(state.Code.Substring(sl, i - sl), state.strict);
                if (!state.Labels.Contains(label._oValue.ToString()))
                    ExceptionHelper.Throw((new SyntaxError("Try to break to undefined label.")));
            }
            else if (!state.AllowBreak.Peek())
                ExceptionHelper.Throw((new SyntaxError("Invalid use of break statement")));
            var pos = index;
            index = i;
            state.breaksCount++;
            return new Break
            {
                Label = label,
                Position = pos,
                Length = index - pos
            };
        }

        public override JSValue Evaluate(Context context)
        {
            context._executionMode = ExecutionMode.Break;
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
            return "break" + (Label != null ? " " + Label : "");
        }
    }
}