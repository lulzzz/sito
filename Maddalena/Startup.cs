using Maddalena.Security;
using Maddalena.Security.Scope;
using Mongolino;
using Maddalena.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;
using AspNetCore.Identity.Mongo;

using ServerSideAnalytics;
using ServerSideAnalytics.Mongo;
using ServerSideAnalytics.Extensions;
using HardwareProviders;
using System.Net;

namespace Maddalena
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            configuration.AddMongolino();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = 32*1024*1024;
                x.MultipartBodyLengthLimit = 64 * 1024 * 1024; // In case of multipart
            });

            services.AddMongoIdentityProvider<ApplicationUser,ApplicationRole>("mongodb://localhost/maddalena", options =>
             {
                 options.Password.RequiredLength = 6;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireDigit = false;
             });

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Program.Configuration["GoogleClientId"];
                googleOptions.ClientSecret = Program.Configuration["GoogleClientSecret"];
            });

            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Program.Configuration["TwitterConsumerKey"];
                twitterOptions.ConsumerSecret =  Program.Configuration["TwitterConsumerSecret"];
            });


            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("blog", policy => policy.Requirements.Add(new DymanicScopeRequirement("blog")));
                options.AddPolicy("manage", policy => policy.Requirements.Add(new DymanicScopeRequirement("blog")));
            });*/

            services.AddScoped<IAuthorizationHandler, DynamicScopeHandler>();


            //Add the transieent to expose the store inside a controller
            //We will use this to show collected data
            services.AddTransient<IAnalyticStore>(x=> GetAnalyticStore());

            services.AddMvc();
        }

        public IAnalyticStore GetAnalyticStore()
        {
            var store = (new MongoAnalyticStore("mongodb://localhost"))
                           .UseIpStackFailOver("IpStackAPIKey")
                           .UseIpApiFailOver()
                           .UseIpInfoFailOver();
            return store;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(()=> 
            {
                Ring0.Close();
                Opcode.Close();
            });

            if (env.IsDevelopment())
            {

            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            app.UseDatabaseErrorPage();

            app.UseAuthentication();

            app.UseServerSideAnalytics(GetAnalyticStore())
               .ExcludePath("/js", // Request into those url space will be not recorded
                            "/lib",
                            "/css")     
               .ExcludeExtension(".jpg", // Request ending with this extension will be not recorded
                                 ".ico",
                                 "robots.txt",
                                 "sitemap.xml")
            
                .Exclude(x => x.UserIdentity() == "matteo")
                .ExcludeIp(IPAddress.Parse("192.168.0.1"))
                .ExcludeLoopBack();          // Request coming from local host will be not recorded

            app.UseStaticFiles();

            app.UseStatusCodePagesWithRedirects("/error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "search",
                    template: "search",
                    defaults: new { controller = "Blog", action = "Search" });

                routes.MapRoute(
                    name: "mynuget",
                    template: "mynuget",
                    defaults: new { controller = "Home", action = "MyNuget" });

                routes.MapRoute(
                    name: "read",
                    template: "read/{link}",
                    defaults: new { controller = "Blog", action = "Read" });

                routes.MapRoute(
                    name: "privacy",
                    template: "privacy",
                    defaults: new { controller = "Home", action = "Privacy" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
