using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreUI.Web.Models;
using Maddalena.Core.GridFs;
using Microsoft.Net.Http.Headers;

namespace CoreUI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGridFileSystem _grid;

        public HomeController(IGridFileSystem grid)
        {
            _grid = grid;
        }

        [Route("/download/{gridName}")]
        public async Task<IActionResult> Download(string gridName)
        {
            if (!await _grid.IsAllowed(gridName, User)) return NotFound();

            var meta = await _grid.GetMetadata(gridName);

            return File(await _grid.Download(gridName), meta.MimeType, meta.FileName, meta.LastModified, EntityTagHeaderValue.Any);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Stat()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public async Task<IActionResult> MyNuget()
        {
            var es = (await Maddalena.Core.Nuget.PackageSearch.GetAsync("Matteo Fabbri"))
                .Data
                .Where(x => x.Authors.Length == 1 && x.Authors[0] == "Matteo Fabbri")
                .OrderBy(x => x.Title)
                .ToArray();

            return View(es);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
