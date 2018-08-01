using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models.Stat;
using Microsoft.AspNetCore.Mvc;
using ServerSideAnalytics;
using ServerSideAnalytics.Extensions;

namespace Maddalena.Controllers
{
    public class StatController : Controller
    {
        readonly IAnalyticStore _store;

        public StatController(IAnalyticStore store)
        {
            _store = store;
        }

        public async Task<ActionResult> Index()
        {
            var from = DateTime.MinValue;
            var to = DateTime.MaxValue;

            var stat = new WebStat
            {
                TotalServed = await _store.CountAsync(from, to),
                UniqueVisitors = await _store.CountUniqueIndentitiesAsync(from, to),
                DailyAverage = await _store.DailyAverage(from, to),
                DailyServed = await _store.DailyServed(from, to),
                HourlyServed = await _store.HourlyServed(from, to),
                ServedByCountry = await _store.ServedByCountry(from, to),
                UrlServed = await _store.UrlServed(from, to),
                Requests = (await _store.InTimeRange(DateTime.Now-TimeSpan.FromMinutes(30), DateTime.Now))
                            .OrderByDescending(x=>x.Timestamp)
            };
            return View(stat);
        }

        public async Task<ActionResult> Identity(string id)
        {
            return View(new WebStat
            {
                Identity = id,
                Requests = (await _store.RequestByIdentityAsync(id)).ToArray(),
            });
        }
    }
}
