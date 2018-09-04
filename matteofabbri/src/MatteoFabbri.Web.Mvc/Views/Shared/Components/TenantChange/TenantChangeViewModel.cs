using Abp.AutoMapper;
using MatteoFabbri.Sessions.Dto;

namespace MatteoFabbri.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
