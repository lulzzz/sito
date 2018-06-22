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
        static readonly MongoRequestRepository repository = new MongoRequestRepository();

        public async Task<ActionResult> Index()
        {
            var thisWeek = DateTime.Now - TimeSpan.FromDays(7);

            return View(new WebStat
            {
                MyId = HttpContext.UserIdentity(),
                Requests = await repository.QueryAsync(x => x.Timestamp >= thisWeek && x.Timestamp <= DateTime.Now),
                UniqueVisitors = await repository.CountUniqueVisitorsAsync(DateTime.MinValue, DateTime.MaxValue)
            });
        }

        public async Task<ActionResult> My()
        {
            var myId = HttpContext.UserIdentity();

            return View(new WebStat
            {
                MyId = myId,
                Requests = await repository.QueryAsync(x => x.Identity == myId),
                UniqueVisitors = await repository.CountUniqueVisitorsAsync(DateTime.MinValue, DateTime.MaxValue)
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
