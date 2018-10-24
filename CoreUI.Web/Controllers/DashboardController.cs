using System.Threading.Tasks;
using Maddalena.Core.Orleans;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansDashboard.Client;

namespace CoreUI.Web.Controllers
{
    public class DashboardController : Controller
    {
        IDashboardClient _client;

        public DashboardController( IOrleansHost host)
        {
            _client = host.Dashboard;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("DashboardCounters")]
        public async Task<IActionResult> DashboardCounters()
        {
            return Json((await _client.DashboardCounters()).Value);
        }

        [Route("ClusterStats")]
        public async Task<IActionResult> ClusterStats()
        {
            return Json((await _client.ClusterStats()).Value);
        }

        [Route("Reminders")]
        public async Task<IActionResult> Reminders()
        {
            return Json((await _client.GetReminders(1, 100)).Value);
        }

        [Route("Reminders/{page}")]
        public async Task<IActionResult> Reminders(int page)
        {
            return Json((await _client.GetReminders(page, 100)).Value);
        }

        [Route("HistoricalStats/{siloGrain}")]
        public async Task<IActionResult> HistoricalStats(string siloGrain)
        {
            return Json((await _client.HistoricalStats(siloGrain)).Value);
        }

        [Route("SiloProperties/{siloGrain}")]
        public async Task<IActionResult> SiloProperties(string siloGrain)
        {
            return Json((await _client.SiloProperties(siloGrain)).Value);
        }

        [Route("SiloStats/{siloGrain}")]
        public async Task<IActionResult> SiloStats(string siloGrain)
        {
            return Json((await _client.SiloStats(siloGrain)).Value);
        }

        [Route("SiloStats/{siloGrain}")]
        public async Task<IActionResult> SiloCounters(string siloGrain)
        {
            return Json((await _client.GetCounters(siloGrain)).Value);
        }

        [Route("GrainStats/{siloGrain}")]
        public async Task<IActionResult> GrainStats(string siloGrain)
        {
            return Json((await _client.GrainStats(siloGrain)).Value);
        }

        [Route("TopGrainMethods")]
        public async Task<IActionResult> TopGrainMethods()
        {
            return Json((await _client.TopGrainMethods()).Value);
        }
    }
}