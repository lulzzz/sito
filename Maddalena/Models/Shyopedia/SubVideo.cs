using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maddalena.Models.Shyopedia
{
    public class SubVideo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Thumbnail { get; set; }
    }
}
