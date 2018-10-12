using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maddalena.Core.Feeds
{
    public interface IFeedService
    {
        Task Create(Feed feed);
        Task<IEnumerable<Feed>> AllFeed();
        Task<IEnumerable<Feed>> AllFeed(string category);
        Task<IEnumerable<string>> Categories();
        Task Update(Feed feed);
        Task Delete(Feed feed);
        Task<Feed> FeedById(string id);
        Task Retrieve(Feed feed, Func<FeedNews, Task> action);
        Task RetrieveAndSave();
    }
}