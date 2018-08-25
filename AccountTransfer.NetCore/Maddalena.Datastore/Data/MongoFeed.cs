using System;
namespace Maddalena.Datastorage.Data
{
    class MongoFeed : MongoBaseObject
    {
        internal string Name { get; set; }

        internal string Url { get; set; }

        internal DateTime LastCheck { get; set; }
    }
}
