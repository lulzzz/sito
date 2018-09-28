using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Maddalena.Core.Npm.Model
{
    internal class UserListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(NpmUserList);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var list = new NpmUserList();
            var obj = JObject.Load(reader);

            list.AddRange(obj.Properties().Select(x => new NpmUser { Name = x.Name }));

            return list;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}