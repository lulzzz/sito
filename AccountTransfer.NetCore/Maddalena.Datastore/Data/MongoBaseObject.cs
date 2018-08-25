using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maddalena.Datastorage.Data
{
    class MongoBaseObject
    {
        [BsonRepresentation(BsonType.ObjectId)]
        internal string Id { get; set; }
    }
}