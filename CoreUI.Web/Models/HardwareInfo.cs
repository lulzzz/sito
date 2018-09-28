using HardwareProviders.Board;
using HardwareProviders.CPU;
using HardwareProviders.HDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class HardwareInfo
    { 
        public Cpu[] Cpu { get; set; }
        public Mainboard Mainboard { get; set; }
        public HardDrive[] Hdd { get; set; }
    }
}
