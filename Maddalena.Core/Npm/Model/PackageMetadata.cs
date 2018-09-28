using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Maddalena.Core.Npm.Model
{
    public class NpmPackageMetadata
    {
        [JsonProperty(PropertyName = "_id")]
        public string NpmId { get; set; }

        [JsonProperty(PropertyName = "_rev")]
        public string Revision { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty(PropertyName = "dist-tags")]
        public Dictionary<string, string> DistibutionTags { get; set; }

        public NpmPackageVersionList Versions { get; set; }

        public NpmUser[] Maintainers { get; set; }
        public NpmUser Author { get; set; }
        public Dictionary<string, DateTime> Time { get; set; }

        public NpmUserList Users { get; set; }

        public string Readme { get; set; }
        public string ReadmeFilename { get; set; }
        public string Homepage { get; set; }
        public string[] Keywords { get; set; }
        public NpmUser[] Contributors { get; set; }
        public Dictionary<string, string> Bugs { get; set; }
        public string License { get; set; }
    }
}