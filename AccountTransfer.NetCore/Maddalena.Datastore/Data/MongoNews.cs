using System;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore.Data
{
    [Serializable]
    class MongoNews : CollectionItem
    {
        internal string Title { get; set; }

        internal string Description { get; set; }

        internal DateTime Timestamp { get; set; }

        internal string[] Categories { get; set; }

        internal string Link { get; set; }
    }
}
