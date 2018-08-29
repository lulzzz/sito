using JS.Core.Core.Interop;

namespace JS.Core.Extensions
{
    internal abstract class IterableProtocolBase
    {
        [Hidden]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [Hidden]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [Hidden]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}