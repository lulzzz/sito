using System.IO;
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
        static void Load()
        {
            var collection = (new MongoClient("mongodb://localhost/"))
                 .GetDatabase("maddalena")
                 .GetCollection<MailMessage>("salvini");

            var directory = @"C:\Users\Administrator\Downloads\New folder\";

            foreach (var item in Directory.GetFiles(directory, "*.eml", SearchOption.AllDirectories))
            {
                try
                {
                    var msg = MimeKit.MimeMessage.Load(item);
                    var body = string.Join("\r\n", msg.BodyParts.OfType<TextPart>().Select(x => x.Text).ToArray());
                    var from = string.Join(";", msg.From.Select(x => x.ToString()));
                    var to = msg.From.Select(x => x.ToString()).ToArray();

                    collection.InsertOne(new MailMessage
                    {
                        Body = body,
                        From = from,
                        To = to,
                        Subject = msg.Subject,
                        FilePath = item
                    });

                    Console.WriteLine(item);
                }
                catch (Exception)
                {
                }
            }

        }

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