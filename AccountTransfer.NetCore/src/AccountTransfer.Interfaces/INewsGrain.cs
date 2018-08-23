using System.Threading.Tasks;
using Orleans;

namespace AccountTransfer.Interfaces
{
    public interface INewsGrain : IGrainWithGuidKey
    {
        Task AnalizeAsync(News news);
        Task UpdateModelAsync();
    }
}
