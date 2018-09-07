using System;
using System.Linq;
using Maddalena.Core.Numl.Serialization;

namespace Maddalena.Core.Numl.Model
{
    public class DateTimePropertySerializer : PropertySerializer
    {
        
        public override bool CanConvert(Type type)
        {
            return typeof(DateTimeProperty).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new DateTimeProperty();
        }

        public override object Read(JsonReader reader)
        {
            var p = (DateTimeProperty)base.Read(reader);

            var features = reader.ReadArrayProperty().Value
                            .Select(o => (string)o)
                            .ToArray();

            p.Features = DateTimeProperty.GetFeatures(features);

            return p;
        }

        public override void Write(JsonWriter writer, object value)
        {
            base.Write(writer, value);
            var p = (DateTimeProperty)value;
            writer.WriteFirstArrayProperty("Features", DateTimeProperty.GetColumns(p.Features));
        }
        
    }
}
