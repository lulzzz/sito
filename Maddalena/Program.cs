using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using WindowsMonitor.CIM.Hardware;
using WindowsMonitor.Win32.Hardware.Cooling;
using WindowsMonitor.Win32.Hardware.Probes;

namespace Maddalena
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}