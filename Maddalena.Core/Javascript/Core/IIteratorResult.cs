namespace Maddalena.Core.Javascript.Core
{
    public interface IIteratorResult
    {
        JSValue value { get; }
        bool done { get; }
    }
}
