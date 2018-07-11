using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maddalena.Models
{
    public class WmiModel
    {
        public string[] Names { get; set; }

        public Type Type { get; set; }

        public PropertyInfo[] Properties { get; set; }

        public IEnumerable<object> Instances { get; set; }
    }
}
