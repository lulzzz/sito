using System.Diagnostics;
using Maddalena.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public IActionResult Collaborators() => View();

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}