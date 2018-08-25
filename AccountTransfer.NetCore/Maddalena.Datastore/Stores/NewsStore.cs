using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Datastorage.Data;
using MongoDB.Driver;

namespace Maddalena.Datastorage
{
    public class NewsStore
    {
        private static readonly IMongoCollection<MongoLabel> _labelCollection =
                                        Datastore.GetCollection<MongoLabel>();

        private static readonly IMongoCollection<MongoNews> _newsCollection = 
                                        Datastore.GetCollection<MongoNews>();

        public async Task<News> Get(string id)
        {
            var mongoNews = await (await _newsCollection.FindAsync(x => x.Id == id))
                    .FirstOrDefaultAsync();
                        
            return mongoNews != null ? Datastore.Map<News>(mongoNews) : null;
        }

        public async Task Create(News news)
        {
            var f = await _newsCollection.FindAsync(x => x.Link == news.Link);

            if (await f.AnyAsync()) return;

            await _newsCollection.InsertOneAsync(Datastore.Map<MongoNews>(news));
        }

        public Task Label(News news, string label, LabelValue labelValue)
        {
            return _labelCollection.InsertOneAsync(new MongoLabel
            {
                Label = label,
                LabelValue = labelValue,
                NewsId = news.Id
            });
        }

        public async Task<LabelValue> GetLabel(News news, string label)
        {
            var f = await (await _labelCollection.FindAsync(x => x.NewsId == news.Id))
                    .FirstOrDefaultAsync();

            return f?.LabelValue ?? LabelValue.Irrelevant;
        }

        public async Task<News[]> GetNews(string label, LabelValue value, int n)
        {
            var options = new FindOptions<MongoLabel, MongoLabel>
            {
                Limit = n
            };

            var labels = await (await _labelCollection
                        .FindAsync(x => x.Label == label && x.LabelValue == value,options))
                        .ToListAsync();

            return labels.Select(async x => await Get(x.NewsId)).Wait().ToArray();
        }
    }
}
