using System.Threading.Tasks;
using System.Linq;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Maddalena.Client;
using System;

namespace Maddalena.Grains.DataProvider
{
    public static class FeedProvider
    {
        public static async Task ReadFeedAsync(Feed feed, Func<News,Task> action)
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

                            var news = new News
                            {
                                Title = item.Title,
                                Description = item.Description,
                                Link = item.Links.First().Uri.AbsoluteUri,
                                Timestamp = item.Published.DateTime,
                                Categories = item.Categories.Select(x => x.Name).ToArray()
                            };

                            await action(news);

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
    }
}
