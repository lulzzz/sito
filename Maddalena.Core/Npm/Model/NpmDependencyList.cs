using System.Collections.Generic;
using Maddalena.Core.Npm.Converters;
using Newtonsoft.Json;

namespace Maddalena.Core.Npm.Model
{
    [JsonConverter(typeof(DependencyListConverter))]
    public class NpmDependencyList : List<NpmDependecyConstraint>
    {
    }
}
