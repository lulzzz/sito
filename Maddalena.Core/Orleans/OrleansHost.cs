using System.Net;
using System.Threading.Tasks;
using Maddalena.Core.Orleans;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Maddalena.Core.Orleans
{
    public class OrleansHost : IOrleansHost
    {
        public OrleansHost()
        {
            StartSilo().Wait();
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Maddalena";
                    options.ServiceId = "Maddalena";
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(TaskGrain).Assembly)
                    .WithReferences();
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .AddStartupTask<TaskGrain>();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
