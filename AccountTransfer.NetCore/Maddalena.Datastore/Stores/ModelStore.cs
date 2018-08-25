using System.Threading.Tasks;
using Maddalena.Datastorage.Data;
using Maddalena.Numl.Serialization;
using Maddalena.Numl.Supervised;
using MongoDB.Driver;


namespace Maddalena.Datastorage.Stores
{
    public class ModelStore
    {
        private static readonly IMongoCollection<MongoModel> _modelCollection =
                                        Datastore.GetCollection<MongoModel>();


        public async Task SaveModelAsync(string name, IModel model)
        {
            var existing = await (await _modelCollection.FindAsync(x => x.Name == name))
                            .FirstOrDefaultAsync();

            if (existing != null)
            {
                existing.Json = JsonWriter.SaveJson(model);
                await _modelCollection.ReplaceOneAsync(x=>x.Id == existing.Id, existing);
            }
            else
            {
                await _modelCollection.InsertOneAsync(new MongoModel
                {
                    Name = name,
                    Json = JsonWriter.SaveJson(model)
                });
            }
        }

        public async Task<IModel> LoadModelAsync(string name)
        {
            var f = await (await _modelCollection.FindAsync(x => x.Name == name))
                            .FirstOrDefaultAsync();

            return f != null ? JsonReader.ReadJson<IModel>(f.Json) : null;
        }
    }
}
