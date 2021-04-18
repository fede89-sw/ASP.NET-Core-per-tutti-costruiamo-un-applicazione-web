using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MyCourse
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);     
            services.AddTransient<ICourseService, EFCoreCourseService>(); 
            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddDbContextPool<MyCourseDbContext>(optionBuilder => {
                optionBuilder.UseSqlite("Data Source=Data/MyCourse.db");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc( routeBuilder => {
                routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); 
            });
        }
    }
}
