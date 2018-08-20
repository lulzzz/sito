using System.Linq;
using HardwareProviders.CPU;
using Maddalena.Models.Hardware;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class HardwareController : Controller
    {
        private static readonly Cpu[] cpus = Cpu.Discover();

        public IActionResult Index()
        {
            foreach (var item in cpus)
            {
                item.Update();
            }

            var data = cpus.Select(cpu => new CpuModel
            {
                Name = cpu.Name,
                Vendor = cpu.Vendor,
                BusClock = cpu.BusClock,
                CoreTemperatures = cpu.CoreTemperatures,
                CorePowers = cpu.CorePowers,
                CoreClocks = cpu.CoreClocks,
                CoreLoads = cpu.CoreLoads,
                CoreCount = cpu.CoreCount,
                TotalLoad = cpu.TotalLoad,
                HasTimeStampCounter = cpu.HasTimeStampCounter,
                PackageTemperature = cpu.PackageTemperature,
                TimeStampCounterFrequency = cpu.TimeStampCounterFrequency
            });
            return View(data);
        }
    }
}