using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Orleans;
using AccountTransfer.Interfaces;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using MongoDB.Driver;
using Orleans.MultiCluster;
using Orleans.Runtime;
using AutoMapper;

namespace AccountTransfer.Grains
{
    [GlobalSingleInstance]
    public class FeedGrain : Grain, IFeedGrain, IRemindable
    {
        private IMapper _mapper;
        private IMongoCollection<MongoFeed> _collection;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            _collection = Mongo.Database.GetCollection<MongoFeed>("feed");

            if(_collection.AsQueryable().Count()==0)
            {
                _collection.InsertOne(new MongoFeed
                {
                    LastCheck = DateTime.Now,
                    Name = "https://thinkprogress.org/feed/",
                    Url = "https://thinkprogress.org/feed/"
                });
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Feed, MongoFeed>();
                cfg.CreateMap<MongoFeed, Feed>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            foreach (var feed in _collection.AsQueryable())
            {
                await ReadFeedAsync(feed);
            }
        }

        public async Task SetupReminderAsync()
        {
            const string reminderName = "reminder";

            try
            {
                if (await GetReminder(reminderName) == null)
                {
                    await RegisterOrUpdateReminder(reminderName, TimeSpan.Zero, TimeSpan.FromMinutes(1));
                }
            }
            catch (Exception e)
            {
            }

        }

        public async Task ReadFeedAsync(Feed feed)
        {
            using (var xmlReader = XmlReader.Create(feed.Url, new XmlReaderSettings() { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        // Read Item
                        case SyndicationElementType.Item:
                            var item = await feedReader.ReadItem();
                            await GrainFactory.GetGrain<INewsGrain>(Guid.NewGuid()).AnalizeAsync(new News
                            {
                                Title = item.Title,
                                Description = item.Description,
                                Link = item.Links.First().Uri.AbsoluteUri,
                                //Timestamp = item.Published.DateTime,
                                Categories = item.Categories.Select(x => x.Name).ToArray(),
                            });

                            break;

                        // Read category
                        case SyndicationElementType.Category:
                            ISyndicationCategory category = await feedReader.ReadCategory();
                            break;

                        // Read Image
                        case SyndicationElementType.Image:
                            ISyndicationImage image = await feedReader.ReadImage();
                            break;

                        // Read link
                        case SyndicationElementType.Link:
                            ISyndicationLink link = await feedReader.ReadLink();
                            break;

                        // Read Person
                        case SyndicationElementType.Person:
                            ISyndicationPerson person = await feedReader.ReadPerson();
                            break;

                        // Read content
                        default:
                            ISyndicationContent content = await feedReader.ReadContent();
                            break;
                    }
                }
            }
        }

        public Task Create(Feed feed)
        {
            return _collection.InsertOneAsync(_mapper.Map<MongoFeed>(feed));
        }

        public Task<Feed[]> GetFeeds()
        {
            var feeds = _collection.AsQueryable().Select(x => _mapper.Map<Feed>(x)).ToArray();
            return Task.FromResult(feeds);
        }

        public Task Update(Feed feed)
        {
            return _collection.FindOneAndReplaceAsync(Builders<MongoFeed>.Filter.Eq(x => x.Url, feed.Url),
                _mapper.Map<MongoFeed>(feed));
        }

        public Task Delete(Feed feed)
        {
            return _collection.FindOneAndDeleteAsync(Builders<MongoFeed>.Filter.Eq(x => x.Url, feed.Url));
        }
    }
}
