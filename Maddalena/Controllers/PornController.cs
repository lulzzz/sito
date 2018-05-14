using Maddalena.Porn;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Mongolino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Controllers
{
    public class PornController : Controller
    {
        static IMongoCollection<PornHubItem> _mongocollection;
        static Collection<PornHubItem> collection;

        static PornController()
        {
            _mongocollection = (new MongoClient("mongodb://localhost/")).GetDatabase("pornhub").GetCollection<PornHubItem>("pornhub");
            collection = new Collection<PornHubItem>("mongodb://localhost/pornhub", "pornhub");
        }

        public async Task<IActionResult> Index(string q)
        {
            var filter = Builders<PornHubItem>.Filter.Text(q ?? "teen", "");
            var res = await _mongocollection.FindAsync(filter, new FindOptions<PornHubItem, PornHubItem>
            {
                BatchSize = 200,
                Limit = 200
            });

            var items = res.ToList().ToArray();

            return View(items);
        }
    }
}
