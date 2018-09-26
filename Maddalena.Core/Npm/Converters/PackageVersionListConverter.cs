using System;
using System.Linq;
using Maddalena.Core.Npm.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Maddalena.Core.Npm.Converters
{
    class PackageVersionListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var list = new NpmPackageVersionList();
            var obj = JObject.Load(reader);

            list.AddRange(obj.Properties().Select(prop => serializer.Deserialize<NpmPackageVersion>(prop.Value.CreateReader())));

            return list;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(NpmPackageVersionList);
    }
}