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
            var @delegate = new Action<JSValue>(text =>
            {
                switch (text.ValueType)
                {
                    case JSValueType.NotExists:
                        break;
                    case JSValueType.NotExistsInObject:
                        break;
                    case JSValueType.Undefined:
                        break;
                    case JSValueType.Boolean:
                        break;
                    case JSValueType.Integer:
                        break;
                    case JSValueType.Double:
                        break;
                    case JSValueType.String:
                        break;
                    case JSValueType.Symbol:
                        break;
                    case JSValueType.Object:
                        break;
                    case JSValueType.Function:
                        break;
                    case JSValueType.Date:
                        break;
                    case JSValueType.Property:
                        break;
                    case JSValueType.SpreadOperatorResult:
                        break;
                }
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