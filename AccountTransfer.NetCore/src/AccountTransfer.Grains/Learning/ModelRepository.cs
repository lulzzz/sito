using System.Threading.Tasks;
using AccountTransfer.Grains;
using Maddalena.News.Grains.Libraries.Mongolino;
using numl.Serialization;
using numl.Supervised;

namespace Maddalena.News.Grains.Learning
{
    static class ModelRepository
    {
        private static readonly Collection<MongoModel> Models = Settings.GetCollection<MongoModel>();

        public static async Task SaveModelAsync(string name, IModel model)
        {
            var existing = await Models.FirstOrDefaultAsync(x => x.Name == name);

            if (existing != null)
            {
                existing.Json = JsonWriter.SaveJson(model);
                await Models.ReplaceAsync(existing);
            }
            else
            {
                await Models.InsertOneAsync(new MongoModel
                {
                    Name = name,
                    Json = JsonWriter.SaveJson(model)
                });
            }
        }

        public static async Task<IModel> LoadModelAsync(string name)
        {
            var f = await Models.FirstOrDefaultAsync(x => x.Name == name);
            return f != null ? JsonReader.ReadJson<IModel>(f.Json) : null;
        }
    }
}
