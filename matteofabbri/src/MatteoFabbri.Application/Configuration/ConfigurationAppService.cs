using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MatteoFabbri.Configuration.Dto;

namespace MatteoFabbri.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MatteoFabbriAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
