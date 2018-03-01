using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Maddalena
{
    public class GridFS
    {
        static GridFS()
        {
            var client = new MongoClient();
            var db = client.GetDatabase("grid");
            Bucket = new GridFSBucket(db);
        }

        public static GridFSBucket Bucket { get; }
    }
}