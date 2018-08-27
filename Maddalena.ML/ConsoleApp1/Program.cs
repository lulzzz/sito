using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Maddalena.ML.DataProvider.Maddalena.Grains.DataProvider;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var feed = new Feed()
            {
                Name = "ansia",
                Url = "http://www.ansa.it/sito/notizie/economia/economia_rss.xml"
            };

            FeedProvider.ReadFeedAsync(feed, x =>
            {
                Console.WriteLine(x.Title);
                Console.WriteLine(x.Description);

                return Task.CompletedTask;
            }).Wait();

            Console.ReadLine();
        }
    }
}
