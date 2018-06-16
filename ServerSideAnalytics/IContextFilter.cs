using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSideAnalytics
{
    public interface IContextFilter
    {
        bool IsRelevant(HttpContext context);
    }
}
