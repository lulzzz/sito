using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreUI.Web.Services;
using Maddalena.Core.Blog.Services;
using Maddalena.Core.Identity;
using Maddalena.Core.Identity.Model;
using Maddalena.Core.Identity.Mongo;
using ServerSideAnalytics.Mongo;
using ServerSideAnalytics;
using ServerSideAnalytics.Extensions;
using System.Net;
using Maddalena.Core.GridFs;

namespace CoreUI.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public MongoAnalyticStore GetAnalyticStore()
        {
            var store = (new MongoAnalyticStore("mongodb://localhost/"));
            return store;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityMongoDbProvider<MaddalenaUser, MongoRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

            }, dbOptions =>
            {
                dbOptions.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                dbOptions.UsersCollection = "Users"; // this is the default value;
                dbOptions.RolesCollection = "Roles"; // this is the default value;

            });


            services.AddAntiforgery();

            services.AddTransient<IGridFileSystem, GridFileSystem>(provider =>
                new GridFileSystem(Configuration.GetConnectionString("DefaultConnection"), "gridFsTable"));

            services.AddTransient<IAnalyticStore, MongoAnalyticStore>(provider => GetAnalyticStore());

            services.AddTransient<IBlogService, MongoBlogService>(provider => new MongoBlogService(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IBlogSettings, BlogSettings>();
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseServerSideAnalytics(GetAnalyticStore()
                           .UseIpStackFailOver("IpStackAPIKey")
                           .UseIpApiFailOver()
                           .UseIpInfoFailOver())

               .ExcludePath("/js", "/lib", "/css")
               .ExcludeExtension(".jpg", ".ico", "robots.txt", "sitemap.xml")

                .Exclude(x => x.UserIdentity() == "matteo")
                .ExcludeIp(IPAddress.Parse("192.168.0.1"))
                .ExcludeLoopBack();


            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
