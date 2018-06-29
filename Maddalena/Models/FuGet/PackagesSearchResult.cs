using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Maddalena.Models.FuGet
{
    public class PackagesSearchResult
    {
        public string PackageId { get; set; } = "";
        public string Version { get; set; } = "";
        public string Description { get; set; } = "";
        public string IconUrl { get; set; } = "";
        public int TotalDownloads { get; set; }
        public string Authors { get; set; } = "";

        public string SafeIconUrl => string.IsNullOrEmpty(IconUrl) ? "/images/no-icon.png" : IconUrl;

        public override string ToString() => PackageId;


        public static async Task<IEnumerable<PackagesSearchResult>> GetAsync(string q)
        {
            // System.Console.WriteLine($"DOWNLOADING {package.DownloadUrl}");
            var queryUrl = $"https://api-v2v3search-0.nuget.org/query?prerelease=true&q={Uri.EscapeDataString(q)}";
            var json = await (new HttpClient()).GetStringAsync(queryUrl).ConfigureAwait(false);

            var j = JObject.Parse(json);
            var d = (JArray)j["data"];

            return d.Select(x => new PackagesSearchResult
            {
                PackageId = (string)x["id"],
                Version = (string)x["version"],
                Description = (string)x["description"],
                IconUrl = (string)x["iconUrl"],
                TotalDownloads = (int)x["totalDownloads"],
                Authors = string.Join(", ", x["authors"])
            }).ToArray();
        }
    }
}