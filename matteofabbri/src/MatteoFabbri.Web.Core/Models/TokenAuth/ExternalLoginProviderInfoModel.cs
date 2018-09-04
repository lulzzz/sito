using Abp.AutoMapper;
using MatteoFabbri.Authentication.External;

namespace MatteoFabbri.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
