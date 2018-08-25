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
                News news = null;

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        // Read Item
                        case SyndicationElementType.Item:
                            var item = await feedReader.ReadItem();

                            news = new News
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
                        case SyndicationElementType.Image:
                        case SyndicationElementType.Link:
                        case SyndicationElementType.Person:
                        default:
                            ISyndicationContent content = await feedReader.ReadContent();
                            break;
                    }
                }
            }
        }
    }
}
