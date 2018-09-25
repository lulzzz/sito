using System;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;
using Maddalena.Core.Javascript.Expressions;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    [Prototype(typeof(Function), true), Serializable]
    public sealed class GeneratorFunction : Function
    {
        public override JSValue prototype
        {
            get => null;
            set
            {

            }
        }
        
        public GeneratorFunction(Context context, FunctionDefinition generator)
            : base(context, generator)
        {
            RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
        }

        protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
        {
            if (construct)
                ExceptionHelper.ThrowTypeError("Generators cannot be invoked as a constructor");

            return Context.GlobalContext.ProxyValue(new GeneratorIterator(this, targetObject, arguments));
        }
    }
}
