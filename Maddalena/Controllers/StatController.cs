using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models.Stat;
using Microsoft.AspNetCore.Mvc;
using ServerSideAnalytics;
using ServerSideAnalytics.Mongo;

namespace Maddalena.Controllers
{
    public class StatController : Controller
    {
        static readonly MongoRequestStore repository = new MongoRequestStore();

        public async Task<ActionResult> Index()
        {
            var stat = new WebStat
            {
                UniqueVisitors = await repository.CountUniqueAsync(DateTime.MinValue, DateTime.MaxValue),
                TotalCount = await repository.CountAsync(DateTime.MinValue, DateTime.MaxValue),
                Requests = (await repository.QueryAsync(x=>true)).Take(100)
            };
            return View(stat);
        }

        public async Task<ActionResult> Identity(string id)
        {
            return View(new WebStat
            {
                Identity = id,
                Requests = (await repository.QueryAsync(x => x.Identity == id)).ToArray(),
            });
        }

        public async Task<ActionResult> Identities()
        {
            var res = await repository.DistinctAsync(x => x.Identity, x => true);
            return Json(res);
        }

        public async Task<ActionResult> Countries()
        {
            var res = await repository.QueryAsync(x => true);
            return Json(res);
        }
    }
}
