using System;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    class MongoFeed : CollectionItem
    {
        internal string Name { get; set; }

        internal string Url { get; set; }

        internal DateTime LastCheck { get; set; }
    }
}
