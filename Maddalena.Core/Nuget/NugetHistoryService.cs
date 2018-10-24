using Maddalena.Core.Mongo;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Core.Nuget
{
    public class NugetHistoryService : INugetHistoryService
    {
        IMongoCollection<PackageHistory> history;

        public NugetHistoryService(string connectionString)
        {
            history = MongoUtil.FromConnectionString<PackageHistory>(connectionString, "nugetHistory");
        }

        public async Task<IGrouping<string,Package>[]> RetrieveAsync()
        {
            var packages = await PackageSearch.GetAsync("MatteoFabbri");

            var histories = packages.Data.Select(pack => new PackageHistory
            {
                DateTime = DateTime.Now,
                Package = pack
            });

            foreach (var item in histories)
            {
                var dayStart = item.DateTime.Date;
                var dayEnd = dayStart + TimeSpan.FromDays(1);

                var exists = await history.AnyAsync(x=> x.Package.Id == item.Package.Id
                                    && x.DateTime >= dayStart
                                    && x.DateTime >= dayEnd
                                    && x.Package.TotalDownloads != item.Package.TotalDownloads);

                if (!exists) await history.InsertOneAsync(item);
            }

            var es = packages
                .Data
                .Where(x => x.Authors.Length == 1 && x.Authors[0] == "Matteo Fabbri")
                .GroupBy(x => x.Title.Split('.').First())
                .OrderBy(x => x.Key)
                .ToArray();

            return es;
        }
    }
}
