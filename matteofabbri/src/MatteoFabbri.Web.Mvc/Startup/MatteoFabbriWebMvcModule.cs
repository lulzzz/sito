using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatteoFabbri.Configuration;

namespace MatteoFabbri.Web.Startup
{
    [DependsOn(typeof(MatteoFabbriWebCoreModule))]
    public class MatteoFabbriWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MatteoFabbriWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<MatteoFabbriNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MatteoFabbriWebMvcModule).GetAssembly());
        }
    }
}
