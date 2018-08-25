using System;
namespace Maddalena.Datastorage.Data
{
    class MongoFeed : MongoBaseObject
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime LastCheck { get; set; }
    }
}
