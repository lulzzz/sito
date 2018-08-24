using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ConfigurationSection = Maddalena.Datastore.Mongolino.Configuration.ConfigurationSection;

namespace Maddalena.Datastore.Mongolino
{
    internal static class Extensions
    {
        internal static Dictionary<string, ConfigurationSection> TypesConfiguration { get; private set; } = new Dictionary<string, ConfigurationSection>();

        internal static IConfiguration AddMongolino(this IConfiguration configuration)
        {
            var section = configuration.GetSection("Mongolino");

            if (section != null && section.Exists())
            {
                TypesConfiguration = section.GetChildren()
                           .Select(sub => new
                           {
                               Type = sub.Key,
                               Section = sub.Get<ConfigurationSection>()
                           })
                          .ToDictionary(x => x.Type, x => x.Section);
            }

            return configuration;
        }
    }
}
