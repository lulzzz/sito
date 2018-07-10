using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public async Task<IActionResult> MyNuget()
        {
            var es = (await Maddalena.Models.Nuget.PackageSearch.GetAsync("Matteo Fabbri"))
                .Data
                .Where(x => x.Authors.Length == 1 && x.Authors[0] == "Matteo Fabbri")
                .ToArray();

            return View(es);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}