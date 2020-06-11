using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Niramaya
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }

        public static string Username_sess = "";

        public static byte[] salt = { 25, 2, 92 };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddRouting();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(20 * 60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //services.AddControllersWithViews().AddRazorRuntimeCompilation();

            ConnectionString = Configuration.GetConnectionString("NiramayaDBcon");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
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

            //app.UseRouting();

            //app.UseAuthorization();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //    //pattern: "{controller=Profile}/{action=ProfileIndexView}/{id?}");
            //    //pattern: "{controller=Home}/{action=Login}/{id?}");
            //    //pattern: "{controller=Home}/{action=Book}/{id?}");
            //    pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
        public void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            //Home/Index 
            routeBuilder.MapRoute("Default", "{controller = Home}/{action = Index}/{id?}");
        }
    }
}
