using Maddalena.Client;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    class MongoLabel : CollectionItem
    {
        internal string Label { get; set; }

        internal Label LabelValue { get; set; }

        internal string NewsId { get; set; }
    }
}
