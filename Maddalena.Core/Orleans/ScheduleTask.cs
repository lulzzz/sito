using System;
using System.Threading.Tasks;

namespace Maddalena.Core.Orleans
{
    public class ScheduleTask
    {
        public string Name { get; set; }

        public string Period { get; set; }

        public Func<Task> Body { get; set; }
    }
}