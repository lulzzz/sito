using System.Net;
using System.Threading.Tasks;
using Maddalena.Core.Orleans;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansDashboard.Client;

namespace Maddalena.Core.Orleans
{
    public class OrleansHost : IOrleansHost
    {
        ISiloHost _host;
        IClusterClient _client;
        DashboardClient _dashboard;

        public OrleansHost()
        {
            StartSilo().Wait();
            StartClient().Wait();

            _dashboard = new DashboardClient(GrainFactory);
        }

        public async Task StartSilo()
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
                .UseDashboard(options =>
                {
                    options.HostSelf = false;
                    options.HideTrace = true;
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(EmptyGrain).Assembly)
                    .WithReferences();
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .AddStartupTask<HostStartup>();

            _host = builder.Build();
            await _host.StartAsync();
        }

        public async Task StartClient()
        {
            _client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Maddalena";
                    options.ServiceId = "Maddalena";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await _client.Connect();
        }

        public IGrainFactory GrainFactory => _client;

        public IDashboardClient Dashboard => _dashboard;
    }
}
