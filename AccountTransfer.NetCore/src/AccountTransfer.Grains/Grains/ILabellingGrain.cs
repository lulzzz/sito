using System.Threading.Tasks;
using Maddalena.Client;
using Orleans;

namespace Maddalena.Grains.Grains
{
    internal interface ILabellingGrain : IGrainWithStringKey
    {
        Task LabelAsync(News news);
        Task UpdateModelAsync();
    }
}