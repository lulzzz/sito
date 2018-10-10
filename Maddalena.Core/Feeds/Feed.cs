using System;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Feeds
{
    public class Feed : MongoObject
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Category { get; set; }

        public DateTime LastCheck { get; set; }
    }
}
