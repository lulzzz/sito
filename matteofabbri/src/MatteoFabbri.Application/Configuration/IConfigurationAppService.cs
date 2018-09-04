using System.Threading.Tasks;
using MatteoFabbri.Configuration.Dto;

namespace MatteoFabbri.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
