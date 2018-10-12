using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Maddalena.Core.Mongo;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
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

        public Task<Feed> FeedById(string id) => _feeds.FirstOrDefaultAsync(x => x.Id == id);

        public async Task Retrieve(Feed feed, Action<FeedNews> action)
        {
            using (var xmlReader = XmlReader.Create(feed.Url, new XmlReaderSettings() { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            var item = await feedReader.ReadItem();

                            action(new FeedNews
                            {
                                Title = item.Title,
                                Published = item.Published,
                                Description = item.Description,
                                Contributors = item.Contributors,
                                Categories = item.Categories
                            });

                        break;
                    }
                }
            }
            /*
                ISyndicationCategory category = await feedReader.ReadCategory();
                ISyndicationImage image = await feedReader.ReadImage();
                ISyndicationLink link = await feedReader.ReadLink();
                ISyndicationPerson person = await feedReader.ReadPerson();
                ISyndicationContent content = await feedReader.ReadContent();
             */
        }

    }
}
