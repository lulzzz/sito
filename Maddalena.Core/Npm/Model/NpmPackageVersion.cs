﻿using Newtonsoft.Json;

namespace Maddalena.Core.Npm.Model
{
    public class NpmPackageVersion
    {
        [JsonProperty(PropertyName = "_id")]
        public string NpmId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public NpmVersionNumber Version { get; set; }

        public NpmUser Author { get; set; }

        public NpmDependencyList Dependencies { get; set; }

        public NpmDependencyList DevDependencies { get; set; }

        public NpmUser[] Contributors { get; set; }

        public string[] Keywords { get; set; }

        public NpmCodeRepository Repository { get; set; }

        public string Main { get; set; }

        public NpmVariableObject Directories { get; set; }

        public NpmVariableObject Scripts { get; set; }

        public NpmVariableObject Bin { get; set; }

        public NpmVariableObject Engines { get; set; }

        [JsonProperty(PropertyName = "_nodeSupported")]
        public bool NodeSupported { get; set; }

        [JsonProperty(PropertyName = "_nodeVersion")]
        public string NodeVersion { get; set; }

        [JsonProperty(PropertyName = "_npmVersion")]
        public string NpmVersion { get; set; }

        [JsonProperty(PropertyName = "dist")]
        public NpmVersionDistribution Distribution { get; set; }

        public string Deprecated { get; set; }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}