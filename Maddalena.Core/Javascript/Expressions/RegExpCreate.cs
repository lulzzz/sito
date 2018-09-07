using System;
using JS.Core.Core;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Statements;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class RegExpExpression : Expression
    {
        private readonly string pattern;
        private readonly string flags;

        protected internal override bool ContextIndependent => false;

        internal override bool ResultInTempContainer => false;

        protected internal override PredictedType ResultType => PredictedType.Object;

        public RegExpExpression(string pattern, string flags)
        {
            this.pattern = pattern;
            this.flags = flags;
        }

        public static CodeNode Parse(ParseInfo state, ref int position)
        {
            var i = position;
            if (!Parser.ValidateRegex(state.Code, ref i, false))
                return null;

            string value = state.Code.Substring(position, i - position);
            position = i;

            state.Code = Parser.RemoveComments(state.SourceCode, i);
            var s = value.LastIndexOf('/') + 1;
            string flags = value.Substring(s);
            try
            {
                return new RegExpExpression(value.Substring(1, s - 2), flags); // объекты должны быть каждый раз разные
            }
            catch (Exception e)
            {
                state.message?.Invoke(MessageLevel.Error, i - value.Length, value.Length, string.Format(Messages.InvalidRegExp, value));
                return new ExpressionWrapper(new Throw(e));
            }
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return null;
        }

        public override JSValue Evaluate(Context context)
        {
            return new RegExp(pattern, flags);
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "/" + pattern + "/" + flags;
        }
    }
}
