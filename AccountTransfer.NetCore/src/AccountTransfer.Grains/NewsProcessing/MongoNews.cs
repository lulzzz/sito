using AccountTransfer.Interfaces;
using MongoDB.Bson;

namespace AccountTransfer.Grains.NewsProcessing
{
    class MongoNews : News
    {
        public ObjectId _id { get; set; }
    }
}
