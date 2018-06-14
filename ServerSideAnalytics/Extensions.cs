using System;
using Microsoft.AspNetCore.Builder;

namespace ServerSideAnalytics
{
    public static class Extensions
    {
        public static IApplicationBuilder UseServerSideAnalytics<T>(this IApplicationBuilder app, IWebRequestRepository<T> repository) where T : IWebRequest
        {
            app.Use(async (context, next) =>
            {
                var req = repository.GetNew();
                req.Timestamp = DateTime.Now;
                req.SessionId = context.Connection.Id;
                req.RemoteIpAddress = context.Connection.RemoteIpAddress.ToString();
                req.User = context.User?.Identity?.Name;
                req.Method = context.Request.Method;
                req.UserAgent = new UserAgent(context.Request.Headers["User-Agent"]);
                req.Path = context.Request.Path.Value;

                await repository.AddAsync(req);

                await next.Invoke();
            });
            return app;
        }
    }
}
