using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ef.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCourse.Models.Entities;

namespace EF_Code_First_setup_example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using(var client = new MyCourseContext())
            {
                // creo database all'avvio dell'app se questo non esiste;
                // se esiste non esegue nulla.
                client.Database.EnsureCreated();
            }
            using (var ctx = new MyCourseContext())
            {
                // setto dei valori che verranno messi nel Database nella taballe 'Courses'
                var course = new Courses() { Id = 1, Title = "Prova1",
                                           Description="random Description", Author = "Mario Rossi" };
        
                ctx.Courses.Add(course);
                ctx.SaveChanges();                
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // aggiungo i servizi di SQLite forniti da EF
            services.AddEntityFrameworkSqlite().AddDbContext<MyCourseContext>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
