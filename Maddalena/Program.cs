using System;
using Jint;
using Jurassic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using MongoDB.Driver;
using Maddalena.Models.Salvini;
using MimeKit;
using System;

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