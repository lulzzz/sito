using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maddalena.Core.Npm.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Maddalena.Core.Npm.Model
{
    [JsonConverter(typeof(VariableObjectConverter))]
    public class NpmVariableObject
    {
        private readonly Dictionary<string, string> _values;

        internal NpmVariableObject(JObject jObject)
        {
            _values = jObject.Properties().ToDictionary(x => x.Name, x => x.Value.ToString());
        }

        internal NpmVariableObject(string v)
        {
            _values = new Dictionary<string, string> { { "0", v } };
        }

        public string[] Keys => _values.Keys.ToArray();

        public string[] Values => _values.Keys.ToArray();

        public string this[string index]
        {
            get => _values[index];
            set => _values[index] = value;
        }
    }
}
