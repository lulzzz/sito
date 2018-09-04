using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace MatteoFabbri.Web.Views
{
    public abstract class MatteoFabbriRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected MatteoFabbriRazorPage()
        {
            LocalizationSourceName = MatteoFabbriConsts.LocalizationSourceName;
        }
    }
}
