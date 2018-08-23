using System.Threading.Tasks;
using Orleans;

namespace AccountTransfer.Interfaces
{
    public interface IFeedGrain : IGrainWithGuidKey
    {
        Task SetupReminderAsync();

        Task Create(Feed feed);
        Task<Feed[]> GetFeeds();
        Task Update(Feed feed);
        Task Delete(Feed feed);
    }
}
