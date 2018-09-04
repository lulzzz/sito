using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatteoFabbri.Authorization;

namespace MatteoFabbri
{
    [DependsOn(
        typeof(MatteoFabbriCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MatteoFabbriApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MatteoFabbriAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MatteoFabbriApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
