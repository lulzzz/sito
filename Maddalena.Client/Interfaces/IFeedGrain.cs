using System.Threading.Tasks;
using Orleans;

namespace Maddalena.Client.Interfaces
{
    public interface IFeedGrain : IGrainWithGuidKey
    {
        Task SetupReminderAsync();
    }
}
