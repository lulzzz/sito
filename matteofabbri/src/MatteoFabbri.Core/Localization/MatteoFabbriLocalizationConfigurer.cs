using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace MatteoFabbri.Localization
{
    public static class MatteoFabbriLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(MatteoFabbriConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MatteoFabbriLocalizationConfigurer).GetAssembly(),
                        "MatteoFabbri.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
