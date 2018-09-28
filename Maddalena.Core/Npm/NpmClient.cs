using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Maddalena.Core.Npm.Model;
using Newtonsoft.Json;

namespace Maddalena.Core.Npm
{
    public class NpmClient
    {
        public static async Task<NpmPackageMetadata> MetadataAsync(string package)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync($"https://registry.npmjs.org/{package}/");
                var K = JsonConvert.DeserializeObject<NpmPackageMetadata>(json);
                return K;
            }
        }

        public static async Task<NpmPackageVersion> LatestVersion(string package)
        {
            var meta = await MetadataAsync(package);
            return meta.Versions.OrderByDescending(x => x.Version).First();
        }

        public static async Task ResolveDependenciesCascade(NpmPackageVersion version, Func<NpmPackageVersion, Task> @forEach)
        {
            var dependecies = await ResolveDependencies(version);

            if ((dependecies?.Length ?? 0) == 0) return;

            foreach (var item in dependecies)
            {
                await forEach(item);

                foreach (var sub in await ResolveDependencies(item))
                {
                    await forEach(item);
                }
            }
        }

        public static async Task<NpmPackageVersion[]> ResolveDependencies(NpmPackageVersion version)
        {
            var versionList = new List<NpmPackageVersion>();

            if ((version.Dependencies?.Count ?? 0) == 0) return new NpmPackageVersion[0];

            foreach (var constraints in version.Dependencies.GroupBy(x=>x.Name))
            {
                IEnumerable<NpmPackageVersion> dependencyVersions =
                    (await MetadataAsync(constraints.Key)).Versions;

                foreach(var c in constraints)
                {
                    dependencyVersions = c.Apply(dependencyVersions);
                }

                var vv = dependencyVersions.First();
                versionList.Add(vv);
            }

            return versionList.ToArray();
        }

        public static async Task Download(NpmPackageVersion version, string folder)
        {
            using (var httpClient = new HttpClient())
            {
                var inStream = await httpClient.GetStreamAsync(version.Distribution.Tarball);

                Stream gzipStream = new GZipInputStream(inStream);

                var tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
                tarArchive.ExtractContents(Path.Combine(folder, version.Name, version.Version.ToString()));
                tarArchive.Close();

                gzipStream.Close();
                inStream.Close();
            }
        }
    }
}