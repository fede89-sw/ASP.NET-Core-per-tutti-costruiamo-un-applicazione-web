using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Usiamo AdoNet o EF per l'accesso ai dati?
            services.AddTransient<ICourseService, AdoNetCourseService>();  
            // services.AddTransient<ICourseService, EFCoreCourseService>(); 

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseResponseCaching();

            app.UseMvc( routeBuilder => {
                routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); 
            });
        }
    }
}
