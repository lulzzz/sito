using System.Threading.Tasks;
using Maddalena.Models.FuGet;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class FugetController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await PackagesSearchResult.GetAsync("Matteo Fabbri"));
        }
    }
}