using System.Collections.Generic;
using System.Threading.Tasks;
using Maddalena.Core.Mongo;
using MongoDB.Driver;

namespace Maddalena.Core.Feeds
{
    public class MongoFeedService : IFeedService
    {
        private readonly IMongoCollection<Feed> _feeds;

        public MongoFeedService(string connectionString)
        {
            _feeds = MongoUtil.FromConnectionString<Feed>(connectionString, "feeds");
        }

        public Task Create(Feed feed) => _feeds.InsertOneAsync(feed);

        public Task<IEnumerable<Feed>> All() => _feeds.WhereAsync(x => true);

        public Task<IEnumerable<Feed>> All(string category) => _feeds.WhereAsync(x => x.Category == category);

        public async Task<IEnumerable<string>> Categories() => (await _feeds.DistinctAsync(x => x.Category, x => true)).ToEnumerable();

        public Task Update(Feed feed) => _feeds.ReplaceOneAsync(x=>x.Id == feed.Id, feed);

        public Task Delete(Feed feed) => _feeds.DeleteOneAsync(x => x.Id == feed.Id);

        public Task<Feed> ById(string id) => _feeds.FirstOrDefaultAsync(x => x.Id == id);
    }
}
