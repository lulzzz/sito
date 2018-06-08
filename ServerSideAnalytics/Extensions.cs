using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace ServerSideAnalytics
{
    public static class Extensions
    {
        static List<WebRequest> requests = new List<WebRequest>();

        public static IApplicationBuilder UseServerSideAnalytics(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var req = new WebRequest
                {
                    Timestamp = DateTime.Now,
                    SessionId = context.Connection.Id,
                    RemoteIpAddress = context.Connection.RemoteIpAddress.ToString(),
                    User = context.User?.Identity?.Name,
                    Method = context.Request.Method,
                    UserAgent = new UserAgent(context.Request.Headers["User-Agent"]),
                    Path = context.Request.Path.Value,
                };
                requests.Add(req);

                await next.Invoke();
            });
            return app;
        }
    }
}
