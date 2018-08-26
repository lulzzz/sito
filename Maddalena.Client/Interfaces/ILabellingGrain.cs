using System.Threading.Tasks;
using Maddalena.Client;
using Orleans;

namespace Maddalena.Client.Interfaces
{
    public interface ILabellingGrain : IGrainWithStringKey
    {
        Task LabelAsync(News news);
        Task SetupReminderAsync();
    }
}