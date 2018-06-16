using System.Collections.Generic;
using System.Linq;
using HardwareProviders.CPU;
using Maddalena.Models.Hardware;
using Microsoft.AspNetCore.Mvc;

namespace Maddalena.Controllers
{
    public class HardwareController : Controller
    {
        static readonly IEnumerable<Cpu> cpus = Cpu.Discover();

        public IActionResult Index()
        {
            var data = cpus.Select(cpu =>
            {
                cpu.Update();
                return new CpuModel
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
                };
            });
            return View(data);
        }
    }
}