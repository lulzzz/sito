using System.Threading.Tasks;
using Maddalena.ML.MachineLearning.Numl.Supervised;
using Maddalena.Numl.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Maddalena.ML.MachineLearning
{
    public class MongoModel
    {
        private static readonly IMongoCollection<MongoModel> ModelCollection = Engine.GetCollection<MongoModel>();

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Json { get; set; }

        [BsonIgnore]
        public IModel Model { get; set; }

        public async Task SaveAsync(IModel model)
        {
            Json = JsonWriter.SaveJson(model);

            if (Id != null)
            {
                await ModelCollection.ReplaceOneAsync(x => x.Id == Id, this);
            }
            else await ModelCollection.InsertOneAsync(this);
        }

        public async Task<MongoModel> LoadAsync(string name)
        {
            var f = await (await ModelCollection.FindAsync(x => x.Name == name))
                .FirstOrDefaultAsync();

            if (f != null)
            {
                f.Model = JsonReader.ReadJson<IModel>(f.Json);
            }

            return f;
        }
    }
}