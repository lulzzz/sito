using System.Threading;
using System.Threading.Tasks;

namespace Maddalena.Core.Orleans
{
    interface ITaskGrain
    {
        Task Execute(CancellationToken cancellationToken);
    }
}