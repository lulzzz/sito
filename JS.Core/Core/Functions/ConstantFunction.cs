using JS.Core.Core.Interop;
using JS.Core.Expressions;
using NiL.JS.BaseLibrary;

namespace JS.Core.Core.Functions
{
    [Prototype(typeof(Function), true)]
    internal sealed class ConstantFunction : Function
    {
        private readonly JSValue _value;

        public ConstantFunction(JSValue value, FunctionDefinition functionDefinition)
            : base(Context.CurrentGlobalContext, functionDefinition)
        {
            _value = value;
        }

        internal override JSValue InternalInvoke(JSValue targetObject, Expression[] arguments, Context initiator, bool withSpread, bool construct)
        {
            for (var i = 0; i < arguments.Length; i++)
                arguments[i].Evaluate(initiator);

            if (construct)
                return base.ConstructObject();

            return _value;
        }

        protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
        {
            return _value;
        }
    }
}
