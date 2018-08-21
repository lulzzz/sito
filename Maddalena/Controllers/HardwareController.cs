using System.Linq;
using HardwareProviders.Board;
using HardwareProviders.CPU;
using HardwareProviders.HDD;
using Maddalena.Models.Hardware;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class HardwareController : Controller
    {
        private static readonly Mainboard mainboard = new Mainboard();
        private static readonly Cpu[] cpus = Cpu.Discover();
        private static readonly HardDrive[] hdd = HardDrive.Discover();

        public IActionResult Index()
        {
            foreach (var item in cpus)
            {
                item.Update();
            }


            return View(new HardwareModel
            {
                Mainboard = mainboard,
                Cpu = cpus,
                Hdd = hdd
            });
        }
    }
}