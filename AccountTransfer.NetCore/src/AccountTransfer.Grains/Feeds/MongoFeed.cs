using AccountTransfer.Interfaces;
using MongoDB.Bson;

namespace AccountTransfer.Grains
{
    class MongoFeed : Feed
    {
        public ObjectId _id { get; set; }
    }
}
