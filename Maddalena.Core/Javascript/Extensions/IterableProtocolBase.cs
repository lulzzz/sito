using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.Extensions
{
    internal abstract class IterableProtocolBase
    {
        [Hidden]
        public override bool Equals(object obj) => base.Equals(obj);

        [Hidden]
        public override int GetHashCode() => base.GetHashCode();

        [Hidden]
        public override string ToString() => base.ToString();
    }
}