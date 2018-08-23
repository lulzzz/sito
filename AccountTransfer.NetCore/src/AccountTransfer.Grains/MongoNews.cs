using AccountTransfer.Interfaces;
using MongoDB.Bson;

namespace AccountTransfer.Grains
{
    class MongoNews : News
    {
        public ObjectId _id { get; set; }
    }
}
