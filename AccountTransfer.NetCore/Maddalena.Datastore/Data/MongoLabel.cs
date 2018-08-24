using Maddalena.Client;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    class MongoLabel : CollectionItem
    {
        public string Name { get; set; }

        public LabelValue Value { get; set; }
    }
}
