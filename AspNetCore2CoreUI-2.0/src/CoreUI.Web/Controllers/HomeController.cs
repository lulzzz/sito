using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreUI.Web.Models;
using HardwareProviders.CPU;
using HardwareProviders.Board;
using HardwareProviders.HDD;
using Maddalena.Core.Blog.Services;
using HardwareProviders;
using System.Security.Permissions;

namespace CoreUI.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Mainboard mainboard = new Mainboard();
        private static Cpu[] cpus;
        private static readonly HardDrive[] hdd = HardDrive.Discover();

        
        static HomeController()
        {
            Load();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        static void Load()
        {
            Ring0.Open();
            HardwareProviders.Opcode.Open();
            Console.WriteLine(Ring0.IsOpen);

            cpus = Cpu.Discover();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Stat()
        {
            return View();
        }

        public ActionResult Identity(string id)
        {
            return View(id as object);
        }

        public IActionResult Hardware()
        {
            foreach (var item in cpus)
            {
                item.Update();
            }
            return View(new HardwareInfo
                {
                    Cpu = cpus,
                    Hdd = hdd,
                    Mainboard = mainboard
                });
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
