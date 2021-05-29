using example.Admin.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace example.Admin
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
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(3);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/user/login";
                options.AccessDeniedPath = "/home/error";
                options.ReturnUrlParameter = "next";
            }).AddGoogle(options => {
                options.ClientSecret = "tjE8wZxm8gcBinAmNVdLKt0S";
                options.ClientId = "732655072130-ihfh67msog7afqph0v36b596vdunmakr.apps.googleusercontent.com";
            });

            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserApiClient, UserApiClient>();
            services.AddSingleton<TelemetryClient, TelemetryClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/error/404";
                    await next();
                }
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //https://rehansaeed.com/seo-friendly-urls-asp-net-core/
                //https://stackoverflow.com/questions/63397267/custom-route-and-action-names-in-asp-net-core-mvc

                //endpoints.MapControllerRoute(
                //name: "user",
                //pattern: "user/forgot-password/{title}",
                //defaults: new { controller = "User", action = "ForgotPassword" });
            });
        }
    }
}
