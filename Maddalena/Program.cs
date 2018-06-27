using System;
using Jint;
using Jurassic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NiL.JS.Core;

namespace Maddalena
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var @delegate = new Action<JSValue>(text =>
            {
                Console.WriteLine(text.ToString());
            });
            var context = new Context();

            context.DefineVariable("alert").Assign(JSValue.Marshal(@delegate));
            context.Eval(@"alert({type:'Fiat', model: new Date(), color:'white'})"); // Message box: Hello, World!

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}