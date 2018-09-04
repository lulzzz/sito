using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MatteoFabbri.MultiTenancy.Dto;

namespace MatteoFabbri.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
