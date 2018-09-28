using System.Collections.Generic;
using System.Linq;
using Maddalena.Core.Npm.Converters;
using Newtonsoft.Json;

namespace Maddalena.Core.Npm.Model
{
    [JsonConverter(typeof(PackageVersionListConverter))]
    public class NpmPackageVersionList : List<NpmPackageVersion>
    {
        public NpmPackageVersion this[string v]
        {
            get
            {
                var n = new NpmVersionNumber(v);
                return this.FirstOrDefault(x => x.Version == n);
            }

        }
    }
}
