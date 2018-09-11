using System.Threading.Tasks;
using Maddalena.Core.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Maddalena.Core.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly IMongoCollection<SettingSlot> _collection;

        public SettingsService(string connectionString)
        {
            _collection = MongoUtil.FromConnectionString<SettingSlot>(connectionString, "settings");
        }

        public void Save<T>(T obj)
        {
            var name = typeof(T).FullName;
            var slot = _collection.FirstOrDefault(x => x.Type == name);

            if (slot != null) _collection.DeleteOne(x=>x.Id == slot.Id);

            _collection.InsertOne(new SettingSlot
            {
                Document = BsonDocument.Create(obj),
                Type = typeof(T).FullName
            });
        }

        public T Get<T>() where T : new()
        {
            var name = typeof(T).FullName;
            var obj = _collection.FirstOrDefault(x => x.Type == name);

            if (obj == null)
            {
                return new T();
            }

            return BsonSerializer.Deserialize<T>(obj.Document);
        }
    }
}
