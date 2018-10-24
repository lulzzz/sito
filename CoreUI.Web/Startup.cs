using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreUI.Web.Services;
using Maddalena.Core.Identity;
using ServerSideAnalytics.Mongo;
using ServerSideAnalytics;
using ServerSideAnalytics.Extensions;
using System.Net;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Maddalena.Core;
using Maddalena.Core.Blog;
using Maddalena.Core.Feeds;
using Maddalena.Core.GridFs;
using Maddalena.Core.Settings;
using Maddalena.Core.Youtube;
using Maddalena.Core.Scripts;
using Maddalena.Core.Orleans;
using Maddalena.Core.Nuget;

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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var gridFs = new GridFileSystem(connectionString, "gridFsTable");

            services.AddSingleton<IServiceCollection>(services);
            services.AddAntiforgery();

            services.AddSingleton<INugetHistoryService>(new NugetHistoryService(connectionString));
            services.AddSingleton<IFeedService>(new MongoFeedService(connectionString));
            services.AddSingleton<IYoutubeService>(new YoutubeService(connectionString));
            services.AddSingleton<IGridFileSystem>(gridFs);
            services.AddSingleton<IScriptService>(new MongoScriptService(connectionString, services.BuildServiceProvider()));
            services.AddSingleton<IFeedService>(new MongoFeedService(connectionString));

            var settings = new SettingsService(connectionString);
            services.AddSingleton<ISettingsService>(settings);

            var orleans = new OrleansHost();
            services.AddSingleton<IOrleansHost>(orleans);

            var webSiteSetting = settings.Get<SiteSettings>();

            if (!string.IsNullOrWhiteSpace(webSiteSetting.GoogleClientId) &&
                !string.IsNullOrWhiteSpace(webSiteSetting.GoogleClientSecret))
            {
                services.AddAuthentication().AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = webSiteSetting.GoogleClientId;
                    googleOptions.ClientSecret = webSiteSetting.GoogleClientSecret;
                });
            }

            if (!string.IsNullOrWhiteSpace(webSiteSetting.TwitterConsumerKey) &&
                !string.IsNullOrWhiteSpace(webSiteSetting.TwitterConsumerKey))
            {
                services.AddAuthentication().AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = webSiteSetting.TwitterConsumerKey;
                    twitterOptions.ConsumerSecret = webSiteSetting.TwitterConsumerKey;
                });
            }

            services.AddTransient<IAnalyticStore, MongoAnalyticStore>(provider => GetAnalyticStore());

            services.AddTransient<IBlogService, MongoBlogService>(provider => new MongoBlogService(Configuration.GetConnectionString("DefaultConnection")));
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

                routes.MapRoute(
                    name: "version",
                    template: "version",
                    defaults: new { controller = "Home", action="Version"   });

                routes.MapRoute(
                    name: "TopGrainMethods",
                    template: "TopGrainMethods",
                    defaults: new { controller = "Dashboard", action = "TopGrainMethods" });
            });
        }
    }
}
