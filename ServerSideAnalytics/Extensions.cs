using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ServerSideAnalytics
{
    public static class Extensions
    {
        public static string UserIdentity(this HttpContext context) => context.Request.Cookies.ContainsKey("ai_user") ? context.Request.Cookies["ai_user"] : context.Connection.Id;

        public static IApplicationBuilder UseServerSideAnalytics<T>(this IApplicationBuilder app, IWebRequestRepository<T> repository, IContextFilter filter=null) where T : IWebRequest
        {
            app.Use(async (context, next) =>
            {
                if (filter?.IsRelevant(context) ?? false)
                {
                    var req = repository.GetNew();
                    req.Timestamp = DateTime.Now;

                    var user = context.User?.Identity?.Name;

                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        req.Identity = context.Request.Cookies.ContainsKey("ai_user")
                                                ? context.Request.Cookies["ai_user"]
                                                : context.Connection.Id;
                    }
                    else
                    {
                        req.Identity = user;
                    }

                    req.RemoteIpAddress = context.Connection.RemoteIpAddress.ToString();
                    req.Method = context.Request.Method;
                    req.UserAgent = context.Request.Headers["User-Agent"];
                    req.Path = context.Request.Path.Value;
                    await repository.AddAsync(req);
                }
                await next.Invoke();
            });
            return app;
        }
    }
}
