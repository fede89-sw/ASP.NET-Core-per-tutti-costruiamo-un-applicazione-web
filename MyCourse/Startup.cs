using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MyCourse
{
    public class Startup
    {
        // l'istanza di Configration è solo lettura; ha infatti solo metodo 'get'
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration)
        {
            // costruttore Startup con oggetto IConfiguration per usare il file 'appsettings.json'
            // per impostare li la connection string
            this.Configuration = Configuration;   
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);     
            services.AddTransient<ICourseService, EFCoreCourseService>(); 
            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddDbContextPool<MyCourseDbContext>(optionBuilder => {
                // abbiamo messo la connection string nel file 'appsettings.json' per maggiore sicurezza.
                // in automatico ASP.NET Core sa che se è presente quel file deve andare li a cercarlo quando uno un oggetto IConfiguration
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default"); // GetSection e GetValue<T> sono metodi di IConfiguration
                optionBuilder.UseSqlite(connectionString);
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
