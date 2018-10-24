using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Core.Nuget
{
    public class NuGetVersion
    {
        public Version Version { get; set; }
        public long Downloads { get; set; }
    }
}