using Maddalena.Security;
using Maddalena.Security.DynamicPolicy;
using Mongolino;
using Maddalena.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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
            services.AddIdentityWithMongoStoresUsingCustomTypes<ApplicationUser,ApplicationRole>("mongodb://localhost/maddalena", options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

            }).AddDefaultTokenProviders();

            /*services.AddMongoIdentityProvider<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            });*/

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Dynamic",
                    policy => policy.Requirements.Add(new DynamicPolicy()));
            });
            services.AddSingleton<IAuthorizationHandler, DynamicPolicyHandler<DynamicAccessStore>>();
            
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
                    name: "dropbox",
                    template: "dropbox",
                    defaults: new { controller = "File", action = "Dropbox", id = "" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
