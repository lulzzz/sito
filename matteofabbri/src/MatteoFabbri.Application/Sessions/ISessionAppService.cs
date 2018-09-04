using System.Threading.Tasks;
using Abp.Application.Services;
using MatteoFabbri.Sessions.Dto;

namespace MatteoFabbri.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
