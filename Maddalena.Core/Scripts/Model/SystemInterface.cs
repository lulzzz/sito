using System;
using System.Text;

namespace Maddalena.Core.Scripts.Model
{
    public class SystemInterface
    {
        private readonly IServiceProvider _services;

        private readonly StringBuilder _builder = new StringBuilder();

        public SystemInterface(IServiceProvider services)
        {
            _services = services;
        }

        public object getService(string service)
        {
            var K = Type.GetType(service);
            return _services.GetService(K);
        }

        public void write(string str)
        {
            _builder.Append(str);
        }

        public void writeLine(string str)
        {
            _builder.AppendLine($"{str}<br/>");
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
