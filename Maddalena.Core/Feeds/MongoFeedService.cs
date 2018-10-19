using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Maddalena.Core.Mongo;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using MongoDB.Driver;

namespace Maddalena.Core.Feeds
{
    public class MongoFeedService : IFeedService, IFeedNewsService
    {
        private readonly IMongoCollection<Feed> _feeds;
        private readonly IMongoCollection<FeedNews> _news;

        public MongoFeedService(string connectionString)
        {
            _feeds = MongoUtil.FromConnectionString<Feed>(connectionString, "feeds");
            _news = MongoUtil.FromConnectionString<FeedNews>(connectionString, "news");
        }

        public Task Create(Feed feed) => _feeds.InsertOneAsync(feed);

        public Task Update(FeedNews news)
        {
            return string.IsNullOrWhiteSpace(news.Id)
                ? _news.InsertOneAsync(news)
                : _news.ReplaceOneAsync(x => x.Id == news.Id, news);
        }

        public Task Delete(FeedNews news)
        {
            return _feeds.DeleteOneAsync(x => x.Id == news.Id);
        }

        public async Task<IEnumerable<FeedNews>> AllNews()
        {
            return (await _news.FindAsync(x => true)).ToEnumerable();
        }

        public async Task<IEnumerable<FeedNews>> ByFeed(Feed feed)
        {
            return (await _news.FindAsync(x => true)).ToEnumerable();
        }

        public async Task<IEnumerable<FeedNews>> ByCategory(string category)
        {
            return (await _news.FindAsync(Builders<FeedNews>.Filter.AnyEq(x=>x.Categories,category))).ToEnumerable();
        }

        public async Task<IEnumerable<FeedNews>> ByContributor(string contributor)
        {
            return (await _news.FindAsync(Builders<FeedNews>.Filter.AnyEq(x => x.Contributors, contributor))).ToEnumerable();
        }

        public async Task<IEnumerable<FeedNews>> FullTextSearch(string query)
        {
            return (await _news.FindAsync(Builders<FeedNews>.Filter.Text(query))).ToEnumerable();
        }

        public Task<IEnumerable<Feed>> AllFeed() => _feeds.WhereAsync(x => true);

        public Task<IEnumerable<Feed>> AllFeed(string category) => _feeds.WhereAsync(x => x.Category == category);

        public async Task<IEnumerable<string>> Categories() => (await _feeds.DistinctAsync(x => x.Category, x => true)).ToEnumerable();

        public Task Update(Feed feed) => _feeds.ReplaceOneAsync(x=>x.Id == feed.Id, feed);

        public Task Delete(Feed feed) => _feeds.DeleteOneAsync(x => x.Id == feed.Id);

        public Task<Feed> FeedById(string id) => _feeds.FirstOrDefaultAsync(x => x.Id == id);

        public async Task Retrieve(Feed feed, Func<FeedNews,Task> action)
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

                            await action(new FeedNews
                            {
                                Title = item.Title,
                                Published = item.Published,
                                Description = item.Description,
                                Link = item.Links.FirstOrDefault()?.Uri?.ToString(),
                                Contributors = item.Contributors.Select(x=>x.Name).ToArray(),
                                Categories = item.Categories.Select(x=>x.Name).ToArray(),
                                FeedId = feed.Id
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

        public async Task RetrieveAndSave()
        {
            await _feeds.ForEach(async feed =>
            {
                await Retrieve(feed, async news =>
                {
                    var found = await _news.FindAsync(x => x.Link == news.Link);


                    if (!await found.AnyAsync()) await Update(news);
                });
            });
        }
    }
}
