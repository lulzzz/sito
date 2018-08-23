using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using numl.Serialization;
using numl.Supervised;
using numl.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AccountTransfer.Grains
{
    class Mongo
    {
        static GridFSBucket _bucket = new GridFSBucket(Database);

        public static IMongoDatabase Database
        {
            get
            {
                var url = new MongoUrl("mongodb://localhost/ml");
                var client = new MongoClient(url);

                return client.GetDatabase(url.DatabaseName);
            }
        }

        static IMongoCollection<Model> _collection = Database.GetCollection<Model>("model");

        class Model
        {
            public ObjectId _id { get; set; }
            public string Name { get; set; }
            public string Json { get; set; }
        }

        static Mongo()
        {
            Ject.AddAssembly(typeof(Mongo).Assembly);
        }

        public static async Task SaveModelAsync(string name, IModel model)
        {
            var find = await (await _collection.FindAsync(x => x.Name == name)).FirstOrDefaultAsync();

            if(find != null)
            {
                find.Json = JsonWriter.SaveJson(model);
                await _collection.ReplaceOneAsync(x => x.Name == name, find);
            }
            else
            {
                var save = new Model
                {
                    Json = JsonWriter.SaveJson(model),
                    Name = name
                };
                await _collection.InsertOneAsync(save);
            }
        }

        public static async Task<IModel> LoadModelAsync(string name)
        {
            try
            {
                var f = await _collection.Find(x => x.Name == name).FirstOrDefaultAsync();
                return f != null ? JsonReader.ReadJson<IModel>(f.Json) : null;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
