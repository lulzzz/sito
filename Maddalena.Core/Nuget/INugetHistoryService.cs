using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Core.Nuget
{
    public interface INugetHistoryService
    {
        Task<IGrouping<string, Package>[]> RetrieveAsync();
    }
}