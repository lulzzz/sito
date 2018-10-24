using Orleans;
using OrleansDashboard.Client;

namespace Maddalena.Core.Orleans
{
    public interface IOrleansHost
    {
        IGrainFactory GrainFactory { get; }

        IDashboardClient Dashboard { get; }
    }
}