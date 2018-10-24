using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreUI.Web.Models;
using Maddalena.Core.GridFs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Maddalena.Core.Nuget;
using System;
using Maddalena.Core.Mongo;
using MongoDB.Driver;

namespace CoreUI.Web.Controllers
{
    public class HomeController : Controller
    {
        private IServiceCollection _services;
        private readonly IGridFileSystem _grid;
        private INugetHistoryService _nuget;
        

        public HomeController(IGridFileSystem grid, IServiceCollection services, INugetHistoryService nugetHistory)
        {
            _grid = grid;
            _services = services;
            _nuget = nugetHistory;
        }

        [Route("/download/{gridName}")]
        public async Task<IActionResult> Download(string gridName)
        {
            if (!await _grid.IsAllowed(gridName, User)) return NotFound();

            var meta = await _grid.GetMetadata(gridName);

            return File(await _grid.Download(gridName), meta.MimeType, meta.FileName, meta.LastModified, EntityTagHeaderValue.Any);
        }


        public IActionResult Index() => View();

        public IActionResult Version() => Json(new
        {
            version = typeof(HomeController).Assembly.GetName().Version.ToString()
        });

        public IActionResult Stat() => View();

        public IActionResult Contact() => View();

        public IActionResult Services() => View(_services);

        public async Task<IActionResult> MyNuget() => View(await _nuget.RetrieveAsync());

        public IActionResult Error() => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
