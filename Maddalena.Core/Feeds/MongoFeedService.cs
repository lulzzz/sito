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

        public Task Update(Feed feed) => _feeds.ReplaceOneAsync(x=>x.Id == feed.Id, feed);

        public Task Delete(Feed feed) => _feeds.DeleteOneAsync(x => x.Id == feed.Id);
    }
}
