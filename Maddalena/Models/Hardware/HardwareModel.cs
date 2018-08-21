using HardwareProviders;
using HardwareProviders.Board;
using HardwareProviders.CPU;
using HardwareProviders.HDD;

namespace Maddalena.Models.Hardware
{
    public class HardwareModel
    {
        public Mainboard Mainboard { get; set; }
        public Cpu[] Cpu { get; set; }
        public HardDrive[] Hdd { get; set; }

    }
}
