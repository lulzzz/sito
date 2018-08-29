using JS.Core.Core;

namespace JS.Core.Expressions
{
    public sealed class SuspendableExpression : Expression
    {
        private readonly Expression _prototype;
        private readonly CodeNode[] _parts;

        internal SuspendableExpression(Expression prototype, CodeNode[] parts)
        {
            _prototype = prototype;
            _parts = parts;
        }

        public override JSValue Evaluate(Context context)
        {
            var i = 0;

            if (context._executionMode >= ExecutionMode.Resume)
            {
                i = (int)context.SuspendData[this];
            }

            for (; i < _parts.Length; i++)
            {
                _parts[i].Evaluate(context);
                if (context._executionMode == ExecutionMode.Suspend)
                {
                    context.SuspendData[this] = i;
                    return null;
                }
            }

            var result = _prototype.Evaluate(context);
            if (context._executionMode == ExecutionMode.Suspend)
            {
                context.SuspendData[this] = i;
                return null;
            }

            return result;
        }

        public override string ToString()
        {
            return _prototype.ToString();
        }
    }
}
