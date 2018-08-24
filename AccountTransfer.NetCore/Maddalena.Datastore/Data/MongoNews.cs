using System;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    [Serializable]
    class MongoNews : CollectionItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] Categories { get; set; }

        public string Link { get; set; }
    }
}
