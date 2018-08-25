using System;
using System.Net;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Client.Interfaces;
using Maddalena.Grains.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace SiloHost
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                var client = new ClusterClient();
                await client.StartClientWithRetries();

                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "AccountTransferApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(FeedGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddStartupTask(
                    async (services, cancellation) =>
                    {
                        // Use the service provider to get the grain factory.
                        var grainFactory = services.GetRequiredService<IGrainFactory>();

                        // Get a reference to a grain and call a method on it.
                        var archiveGrain = grainFactory.GetGrain<IFeedGrain>(Guid.Empty);
                        await archiveGrain.SetupReminderAsync();
                    })
                .AddMemoryGrainStorageAsDefault()
                .UseInMemoryReminderService();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
