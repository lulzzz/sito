using NiL.JS.BaseLibrary;

namespace JS.Core.Core
{
    public interface ICallable
    {
        FunctionKind Kind { get; }

        JSValue Construct(Arguments arguments);

        JSValue Construct(JSValue targetObject, Arguments arguments);

        JSValue Call(JSValue targetObject, Arguments arguments);
    }
}
