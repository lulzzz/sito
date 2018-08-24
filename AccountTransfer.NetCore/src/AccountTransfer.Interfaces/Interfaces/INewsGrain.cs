using System.Threading.Tasks;
using Orleans;

namespace Maddalena.Client.Interfaces
{
    public interface INewsGrain : IGrainWithGuidKey
    {
        Task Create(News news);
        Task<News[]> GetNews();
        Task<News[]> NewsInLabel(string label, Label value);
        Task Update(News news);
        Task Delete(News news);
    }
}
