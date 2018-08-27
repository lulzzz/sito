using System;
using Maddalena.Numl.Supervised;
using System.Reflection;
using Maddalena.Numl.Math.LinearAlgebra;
using Maddalena.Numl.Supervised.Regression;

namespace Maddalena.Numl.Serialization.Supervised
{
    public class LinearRegressionSerializer : ModelSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(LinearRegressionModel).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new LinearRegressionModel();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var d = base.Read(reader) as LinearRegressionModel;
                d.Theta = reader.ReadVectorProperty().Value as Vector;
                return d;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var m = value as LinearRegressionModel;
                base.Write(writer, m);
                writer.WriteProperty(nameof(m.Theta), m.Theta);
            }
        }
    }
}
