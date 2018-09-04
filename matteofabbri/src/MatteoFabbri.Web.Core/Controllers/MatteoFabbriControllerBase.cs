using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MatteoFabbri.Controllers
{
    public abstract class MatteoFabbriControllerBase: AbpController
    {
        protected MatteoFabbriControllerBase()
        {
            LocalizationSourceName = MatteoFabbriConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
