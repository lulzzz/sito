using System;
using Maddalena.Numl.Supervised;
using System.Reflection;
using Maddalena.ML.MachineLearning.Numl.Data;
using Maddalena.Numl.Supervised.DecisionTree;


namespace Maddalena.Numl.Serialization.Supervised
{
    public class DecisionTreeSerializer : ModelSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(DecisionTreeModel).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new DecisionTreeModel();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var d = base.Read(reader) as DecisionTreeModel;
                d.Tree = reader.ReadProperty().Value as Tree;
                d.Hint = (double)reader.ReadProperty().Value;
                return d;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var d = value as DecisionTreeModel;
                base.Write(writer, d);
                writer.WriteProperty(nameof(d.Tree), d.Tree);
                writer.WriteProperty(nameof(d.Hint), d.Hint);
            }
        }
    }
    public class NodeSerializer : JsonSerializer<Node> { }
    public class EdgeSerializer : JsonSerializer<Edge> { }
}
