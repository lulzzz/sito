using Maddalena.Datastorage.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maddalena.Datastorage.Stores
{
    public class SettingStore
    {
        private static MongoSettings mongoSettings;
        private static readonly IMongoCollection<MongoSettings> _settingCollection;

        static SettingStore()
        {
            _settingCollection = Datastore.GetCollection<MongoSettings>();

            if(_settingCollection.EstimatedDocumentCount()==0)
            {
                _settingCollection.InsertOne(new MongoSettings()
                {
                    Labels = new List<string>(new[]
                    {
                        "Europe",
                        "USA",
                        "Trump",
                        "Brexit"
                    })
                });

                mongoSettings = _settingCollection.Find(x => true).First();
            }
        }

        public Task<string[]> Labels() => Task.FromResult(mongoSettings.Labels.ToArray());

        public Task AddLabel(string label)
        {
            mongoSettings.Labels.Add(label);
            return _settingCollection.ReplaceOneAsync(x => x.Id == mongoSettings.Id, mongoSettings);
        }

        public Task RemoveLabel(string label)
        {
            mongoSettings.Labels.Remove(label);
            return _settingCollection.ReplaceOneAsync(x => x.Id == mongoSettings.Id, mongoSettings);
        }
    }
}
