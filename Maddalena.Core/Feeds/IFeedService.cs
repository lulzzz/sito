using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maddalena.Core.Feeds
{
    public interface IFeedService
    {
        Task Create(Feed feed);
        Task<IEnumerable<Feed>> All();
        Task Update(Feed feed);
        Task Delete(Feed feed);
    }
}