using ef.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            // passo le opzioni del DbContext direttamente qui sotto; cosi facendo devo avere 
            // in MyCourseContext il costruttore che prende DbContextOptions come parametro.
            services.AddDbContext<MyCourseContext>(optionsBuilder => {
                // setto la 'connection string' per il database
                optionsBuilder.UseSqlite("Filename=Data/EF_CF_example.db");
            });

            // Se le opzioni del DbContext le imposto nel metodo 'OnConfiguring' di MyCourseContext
            // posso commentare AddDbContext qua sopra e usare quello sotto, togliendo anche il 
            // costruttore di MyCourseContext che accetta DbContextOptions come parametro
            // services.AddDbContext<MyCourseContext>();
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
