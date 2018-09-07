namespace JS.Core.Core
{
    public interface IIteratorResult
    {
        JSValue value { get; }
        bool done { get; }
    }
}
