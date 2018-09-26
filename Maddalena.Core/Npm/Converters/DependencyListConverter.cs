using System;
using System.Linq;
using Maddalena.Core.Npm.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Maddalena.Core.Npm.Converters
{
    public class DependencyListConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var list = new NpmDependencyList();
            var obj = JObject.Load(reader);

            foreach (var prop in obj.Properties())
            {
                list.AddRange(DependencyParser.FromString(prop.Value.ToString() , prop.Name));
            }

            return list;
        }

        public override bool CanConvert(Type type) => type == typeof(NpmDependencyList);
    }
}