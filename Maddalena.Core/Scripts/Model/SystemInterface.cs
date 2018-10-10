using System.Text;

namespace Maddalena.Core.Scripts.Model
{
    public class SystemInterface
    {
        private readonly StringBuilder _builder = new StringBuilder();
        
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
