using System.Threading.Tasks;
using Orleans;

namespace Maddalena.News.Grains.Grains
{
    internal interface ILabellingGrain : IGrainWithStringKey
    {
        Task LabelAsync(MongoNews news);
        Task UpdateModelAsync();
    }
}