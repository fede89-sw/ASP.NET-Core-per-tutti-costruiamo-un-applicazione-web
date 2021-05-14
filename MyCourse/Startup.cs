using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MyCourse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;   
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();

            services.AddMvc( options =>
            {
                var homeProfile = new CacheProfile();
                Configuration.Bind("ResponseCache:Home", homeProfile);      
                options.CacheProfiles.Add("Home", homeProfile);
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            // istruzioni per il preprocessore,che agisce subito prima della compilazione
            #if DEBUG
            .AddRazorRuntimeCompilation()
            #endif
            ;

            // Usiamo AdoNet o EF per l'accesso ai dati?
            // services.AddTransient<ICourseService, AdoNetCourseService>();  
            services.AddTransient<ICourseService, EFCoreCourseService>(); 

            services.AddTransient<IDatabaseService, DatabaseService>();

            services.AddTransient<ICachedCourseService, MemoryCacheCourseService>();

            services.AddDbContextPool<MyCourseDbContext>(optionBuilder => {
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");
                optionBuilder.UseSqlite(connectionString);
            });

            services.Configure<ConnectionStringOptions>(Configuration.GetSection("ConnectionStrings"));

            services.Configure<CoursesOptions>(Configuration.GetSection("Courses"));

            services.Configure<CachedLifeOptions>(Configuration.GetSection("CachedLife"));

            services.Configure<MemoryCacheOptions>(Configuration.GetSection("MemoryCache"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            // Endpoint Routing Middleware
            app.UseRouting();

            app.UseResponseCaching();

            // Endpoint Middleware
            app.UseEndpoints(routeBuilder => {
                routeBuilder.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            // app.UseMvc( routeBuilder => {
            //     routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); 
            // });
        }
    }
}
