using Maddalena.Client;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    class NewsLabel : CollectionItem
    {
        public string Label { get; set; }

        public LabelValue LabelValue { get; set; }

        public string NewsId { get; set; }
    }
}
