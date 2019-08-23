using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOE.App.Controllers;
using AOE.App.Services;
using AOE.Application.Base.Cache;
using AOE.Application.Base.Models;
using AOE.Application.Models.Framework;
using AOE.Application.Repositories;
using AOE.Application.Services;
using AOE.Database.Repositories;
using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AOE.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 1;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = false;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            });

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration.GetSection("AppSettings:AuthenticationGoogleClientId").Value;
                googleOptions.ClientSecret = Configuration.GetSection("AppSettings:AuthenticationGoogleClientSecret").Value;
            });

            ConfigureIoC(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureIoC(IServiceCollection services)
        {
            string mongoUrl = Configuration.GetConnectionString("DefaultConnection");
            MongoUrl url = new MongoUrl(mongoUrl);
            IMongoDatabase database = new MongoClient(url).GetDatabase(url.DatabaseName);
            services.AddSingleton(database);
            services.AddScoped<AccountController>();
            services.AddTransient<ICacheManager, MemoryCacheManager>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

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
