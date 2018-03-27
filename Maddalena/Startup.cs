using Maddalena.Security;
using Maddalena.Security.Scope;
using Mongolino;
using Maddalena.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Features;
using AspNetCore.Identity.Mongo;

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
                googleOptions.ClientId = "127806004786-jsnf1cr38s8aps1higamtjivectofv21.apps.googleusercontent.com";
                googleOptions.ClientSecret = "QGrQnAvHGazDN-hM2Aaa2v5f";
            });

            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = "SoLYId2A3LZDmTqoGPshlJZk5";
                twitterOptions.ConsumerSecret = "GDdGYUx9f3i6OPGGvlgSu9Mxk8hgnT98jcK21kqO7J33mvO3tj";
            });


            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("blog", policy => policy.Requirements.Add(new DymanicScopeRequirement("blog")));
                options.AddPolicy("manage", policy => policy.Requirements.Add(new DymanicScopeRequirement("blog")));
            });*/

            services.AddScoped<IAuthorizationHandler, DynamicScopeHandler>();
            
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

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "search",
                    template: "search",
                    defaults: new { controller = "Blog", action = "Search", id = "" });

                routes.MapRoute(
                    name: "read",
                    template: "read",
                    defaults: new { controller = "Blog", action = "Read", id = "" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
