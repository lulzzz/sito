using System.Threading.Tasks;
using Abp.Application.Services;
using MatteoFabbri.Authorization.Accounts.Dto;

namespace MatteoFabbri.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
