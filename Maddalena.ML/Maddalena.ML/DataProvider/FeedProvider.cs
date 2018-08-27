using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Maddalena.ML.Extensions;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace Maddalena.ML.DataProvider
{
    namespace Maddalena.Grains.DataProvider
    {
        public static class FeedProvider
        {
            public static async Task ReadFeedAsync(Feed feed, Func<News, Task> publishAction)
            {
                using (var xmlReader = XmlReader.Create(feed.Url, new XmlReaderSettings { Async = true }))
                {
                    var feedReader = new RssFeedReader(xmlReader);

                    while (await feedReader.Read())
                    {
                        switch (feedReader.ElementType)
                        {
                            // Read Item
                            case SyndicationElementType.Item:
                                var item = await feedReader.ReadItem();
                            
                                await publishAction(new News
                                {
                                    Title = item.Title,
                                    Source = feed.Name,
                                    SourceType = SourceType.Feed,
                                    Description = item.Description.PurgeHtml(),
                                    Link = item.Links.First().Uri.AbsoluteUri,
                                    Timestamp = item.Published.DateTime,
                                    Categories = item.Categories.Select(x => x.Name).ToArray()
                                });

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
}