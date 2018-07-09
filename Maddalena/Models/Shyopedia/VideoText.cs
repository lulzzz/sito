using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Maddalena.Models.Shyopedia
{
    public class VideoText
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string VideoId { get; set; }

        public TimeSpan OffSet { get; set; }

        public string Text { get; set; }
    }
}
