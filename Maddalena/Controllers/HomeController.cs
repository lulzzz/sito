using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public async Task<IActionResult> MyNuget()
        {
            var es = (await Models.Nuget.PackageSearch.GetAsync("Matteo Fabbri"))
                .Data
                .Where(x => x.Authors.Length == 1 && x.Authors[0] == "Matteo Fabbri")
                .OrderByDescending(x => x.Title)
                .ToArray();

            return View(es);
        }

        public IActionResult Error(int code)
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}