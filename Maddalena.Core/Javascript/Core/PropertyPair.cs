using NiL.JS.BaseLibrary;

namespace JS.Core.Core
{
    public sealed class PropertyPair
    {
        internal Function getter;
        internal Function setter;

        public Function Getter => getter;
        public Function Setter => setter;

        internal PropertyPair() { }

        public PropertyPair(Function getter, Function setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public override string ToString()
        {
            var tempStr = "[";
            if (getter != null)
                tempStr += "Getter";
            if (setter != null)
                tempStr += (tempStr.Length != 1 ? "/Setter" : "Setter");
            if (tempStr.Length == 1)
                return "[Invalid Property]";
            tempStr += "]";
            return tempStr;
        }
    }
}
