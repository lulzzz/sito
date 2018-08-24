using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maddalena.Datastore.Mongolino
{
    internal class CollectionItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        internal string Id { get; set; }
    }
}
