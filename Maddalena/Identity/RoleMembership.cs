using Maddalena.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maddalena.Identity
{
    public class RoleMembership : DBObject<RoleMembership>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string RoleId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
