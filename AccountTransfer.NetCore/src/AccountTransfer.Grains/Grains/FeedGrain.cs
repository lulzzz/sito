using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AccountTransfer.Grains;
using AutoMapper;
using Maddalena.Client;
using Maddalena.Client.Interfaces;
using Maddalena.News.Client;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Orleans;
using Orleans.MultiCluster;
using Orleans.Runtime;

namespace Maddalena.News.Grains.Grains
{
    [GlobalSingleInstance]
    public class FeedGrain : Grain, IFeedGrain, IRemindable
    {
        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            foreach (var feed in _feedCollection.All)
            {
                await ReadFeedAsync(feed);
            }
        }

        public async Task SetupReminderAsync()
        {
            const string reminderName = "reminder";

            if (await GetReminder(reminderName) == null)
            {
                await RegisterOrUpdateReminder(reminderName, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            }
        }

        private async Task ReadFeedAsync(MongoFeed feed)
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

                            var news = new MongoNews
                            {
                                Title = item.Title,
                                Description = item.Description,
                                Link = item.Links.First().Uri.AbsoluteUri,
                                Timestamp = item.Published.DateTime,
                                Categories = item.Categories.Select(x => x.Name).ToArray()
                            };

                            if (await _newsCollection.AnyAsync(x => x.Link == news.Link)) continue;

                            foreach (var label in _labelCollection.All)
                            {
                                await GrainFactory.GetGrain<ILabellingGrain>(label.Name).LabelAsync(news);
                            }

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
            return _feedCollection.InsertOneAsync(_mapper.Map<MongoFeed>(feed));
        }

        public Task<Feed[]> GetFeeds()
        {
            var feeds = _feedCollection.All.Select(x => _mapper.Map<Feed>(x)).ToArray();
            return Task.FromResult(feeds);
        }

        public Task Update(Feed feed)
        {
            return _feedCollection.ReplaceAsync(_mapper.Map<MongoFeed>(feed));
        }

        public Task Delete(Feed feed)
        {
            return _feedCollection.DeleteAsync(_mapper.Map<MongoFeed>(feed));
        }
    }
}
