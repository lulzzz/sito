using Abp.AspNetCore.Mvc.ViewComponents;

namespace MatteoFabbri.Web.Views
{
    public abstract class MatteoFabbriViewComponent : AbpViewComponent
    {
        protected MatteoFabbriViewComponent()
        {
            LocalizationSourceName = MatteoFabbriConsts.LocalizationSourceName;
        }
    }
}
