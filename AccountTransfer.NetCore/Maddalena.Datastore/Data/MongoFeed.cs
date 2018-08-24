using System;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    class MongoFeed : CollectionItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime LastCheck { get; set; }
    }
}
