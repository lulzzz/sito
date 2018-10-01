using HardwareProviders.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermalService
{
    class Program
    {
        static void Main(string[] args)
        {
            var cpus = Cpu.Discover();

            foreach (var c in cpus)
            {
                Console.WriteLine(c.CoreTemperatures);
            }
        }
    }
}
