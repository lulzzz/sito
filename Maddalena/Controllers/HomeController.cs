using System.Diagnostics;
using System.Threading.Tasks;
using Maddalena.Models;
using Maddalena.Models.FuGet;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Maddalena.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public async Task<IActionResult> MyNuget()
        {
            var es = (await PackagesSearchResult.GetAsync("Matteo Fabbri"))
                .Where(x => x.Authors == "Matteo Fabbri")
                .ToArray();

            return View(es);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}