using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Maddalena.ML
{
    public class Engine
    {
        internal static ISiloHost SiloHost { get; private set; }

        internal static IMongoCollection<T> GetCollection<T>()
        {
            var url = new MongoUrl("mongodb://localhost/ml");
            return new MongoClient(url).GetDatabase(url.DatabaseName).GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        internal static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "AccountTransferApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(FeedGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddStartupTask(
                    async (services, cancellation) =>
                    {
                        // Use the service provider to get the grain factory.
                        var grainFactory = services.GetRequiredService<IGrainFactory>();
                    })
                .AddMemoryGrainStorageAsDefault()
                .UseInMemoryReminderService();

            SiloHost = builder.Build();
            await SiloHost.StartAsync();
            return SiloHost;
        }

        public static void Setup()
        {
            StartSilo().Wait();
        }
    }
}