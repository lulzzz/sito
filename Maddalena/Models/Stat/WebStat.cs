using ServerSideAnalytics;
using System.Collections.Generic;

namespace Maddalena.Models.Stat
{
    public class WebStat
    {
        public string Identity { get; set; }
        public IEnumerable<IWebRequest> Requests { get; set; }
        public long UniqueVisitors { get; set; }
        public long TotalCount { get; set; }
    }
}
