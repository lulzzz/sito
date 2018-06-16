using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Extensions
{
    public static class DateTimeExtensions
    {
        public static string RelativeDate(this DateTime dt)
        {
            var now = DateTime.Now;

            if ((now - dt).TotalHours < 24) return dt.ToString("HH:mm");

            if ((now - dt).TotalDays < 365) return dt.ToString("dd/MM");

            return dt.ToString("yyyy");
        }
    }
}
