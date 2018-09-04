using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatteoFabbri.Configuration;

namespace MatteoFabbri.Web.Host.Startup
{
    [DependsOn(
       typeof(MatteoFabbriWebCoreModule))]
    public class MatteoFabbriWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MatteoFabbriWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MatteoFabbriWebHostModule).GetAssembly());
        }
    }
}
