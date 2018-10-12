using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maddalena.Core.Feeds
{
    public interface IFeedNewsService
    {
        Task Update(FeedNews news);
        Task Delete(FeedNews news);
        Task<IEnumerable<FeedNews>> AllNews();
        Task<IEnumerable<FeedNews>> ByFeed(Feed feed);
        Task<IEnumerable<FeedNews>> ByCategory(string category);
        Task<IEnumerable<FeedNews>> ByContributor(string contributor);
        Task<IEnumerable<FeedNews>> FullTextSearch(string query);
    }
}
