using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maddalena.Datastore.Mongolino
{
    public class CollectionItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
