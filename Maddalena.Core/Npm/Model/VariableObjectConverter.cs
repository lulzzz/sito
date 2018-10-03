using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maddalena.Core.Npm.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Maddalena.Core.Npm.Converters
{
    class VariableObjectConverter : JsonConverter
    {
       public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return new NpmVariableObject(JObject.Load(reader));
                case JsonToken.String:
                    return new NpmVariableObject(reader.Value.ToString());
                case JsonToken.StartArray:
                    return new NpmVariableObject(JArray.Load(reader));
                default:
                    throw new Exception();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(NpmVariableObject);
        }
    }
}
