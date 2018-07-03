using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Maddalena.Models.Shyopedia
{
    public class VideoText
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string VideoId { get; set; }

        [BsonIgnore]
        public string Title { get; set; }

        [BsonIgnore]
        public string Url { get; set; }

        [BsonIgnore]
        public string Thumbnail { get; set; }

        public double OffSet { get; set; }

        public string Text { get; set; }

        public double Duration { get; set; }
    }
}
