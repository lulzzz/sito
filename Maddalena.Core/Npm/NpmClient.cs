using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Maddalena.Core.Npm.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Maddalena.Core.Npm
{
    public class NpmClient
    {
        public static async Task<NpmPackageMetadata> MetadataAsync(string package)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync($"https://registry.npmjs.org/{package}/");
                return JsonConvert.DeserializeObject<NpmPackageMetadata>(json);
            }
        }

        public static async Task<NpmPackageVersion> LatestVersion(string package)
        {
            var meta = await MetadataAsync(package);
            return meta.Versions.OrderByDescending(x => x.Version).First();
        }

        public static async Task DownloadWithDependencies(string package, string folder)
        {
            await DownloadWithDependencies(await LatestVersion(package), folder);
        }

        public static async Task DownloadWithDependencies(NpmPackageVersion version, string folder)
        {
            Console.WriteLine($"{version.Name} {version.Version}");

            await Download(version, folder);

            if(version.Dependencies == null) return;

            foreach (var dep in version.Dependencies)
            {
                var meta = await MetadataAsync(dep.Name);

                if (meta == null)
                {

                }
                else
                {
                    await DownloadWithDependencies(meta.Versions[dep.Version.ToString()], folder);
                }
            }
        }

        public static async Task Download(NpmPackageVersion version, string folder)
        {
            using (var httpClient = new HttpClient())
            {
                var inStream = await httpClient.GetStreamAsync(version.Distribution.Tarball);

                Stream gzipStream = new GZipInputStream(inStream);

                var tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
                tarArchive.ExtractContents(Path.Combine(folder, version.Name, version.Version));
                tarArchive.Close();

                gzipStream.Close();
                inStream.Close();
            }
        }
    }
}
