using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Datastorage.Data;
using MongoDB.Driver;

namespace Maddalena.Datastorage
{
    public class NewsStore
    {
        private static readonly IMongoCollection<MongoNews> _newsCollection = 
                                        Datastore.GetCollection<MongoNews>();

        public async Task<News[]> AllAsync()
        {
            var ns = await _newsCollection.Find(x => true).ToListAsync();
            return ns.Select(x => Datastore.Map<News>(x)).ToArray();
        }

        public Task DeleteAsync(string id)
        {
            return _newsCollection.FindOneAndDeleteAsync(x => x.Id == id);
        }

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

            var mnews = Datastore.Map<MongoNews>(news);

            await _newsCollection.InsertOneAsync(mnews);

            news.Id = mnews.Id;
        }

        public async Task LabelAsync(News news, string label, LabelValue labelValue)
        {
            var mongoNews = await (await _newsCollection.FindAsync(x => x.Id == news.Id))
                    .FirstOrDefaultAsync();

            if (mongoNews.Bad == null) mongoNews.Bad = new List<string>();
            if (mongoNews.Good == null) mongoNews.Good = new List<string>();

            switch (labelValue)
            {
                case LabelValue.Bad:
                    if (!mongoNews.Bad.Contains(label))
                    {
                        mongoNews.Bad.Add(label);
                    }
                    await _newsCollection.ReplaceOneAsync(x => x.Id == mongoNews.Id, mongoNews);
                    break;
                case LabelValue.Good:
                    if (!mongoNews.Good.Contains(label))
                    {
                        mongoNews.Good.Add(label);
                    }

                    await _newsCollection.ReplaceOneAsync(x => x.Id == mongoNews.Id, mongoNews);
                    break;
            }

        }

        public async Task<LabelValue> GetLabel(News news, string label)
        {
            var f = await (await _newsCollection.FindAsync(x => x.Id == news.Id))
                    .FirstOrDefaultAsync();

            if (f == null) return LabelValue.Irrelevant;

            if (f.Bad?.Contains(label) == true) return LabelValue.Bad;

            return f.Good?.Contains(label) == true ? LabelValue.Good : LabelValue.Irrelevant;
        }

        public async Task<News[]> GetNews(string label, LabelValue value, int n)
        {
            var options = new FindOptions<MongoNews, MongoNews>
            {
                Limit = n
            };

            FilterDefinition<MongoNews> filter = null;

            switch (value)
            {
                case LabelValue.Irrelevant:
                    var fbad = Builders<MongoNews>.Filter.AnyEq(x => x.Bad, label);
                    var fgood = Builders<MongoNews>.Filter.AnyEq(x => x.Good, label);

                    filter = Builders<MongoNews>.Filter.Not(fbad & fgood);
                    break;
                case LabelValue.Bad:
                    filter = Builders<MongoNews>.Filter.AnyEq(x => x.Bad, label);
                    break;
                case LabelValue.Good:
                    filter = Builders<MongoNews>.Filter.AnyEq(x => x.Good, label);
                    break;
            
            }

            if (filter == null) throw new Exception("Something really bad happened here");

            var find = await _newsCollection.FindAsync(filter, options);
            return (await find.ToListAsync())
                    .Select(x=>Datastore.Map<News>(x))
                    .ToArray();
        }
    }
}
