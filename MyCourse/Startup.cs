using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Options;
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
                // questo sotto è il metodo più facile per accedere alle configurazioni di 'appsettings.json' senza creare una nuova classe,
                // cosa che abbiamo cmq fatto con ConnectionStringOptions.cs
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default"); // GetSection e GetValue<T> sono metodi di IConfiguration
                optionBuilder.UseSqlite(connectionString);
            });
            // utilizzo la classe ConnectionStringOptions per avere la connectionString dal file appsettings.json in maniera fortemente tipizzata
            services.Configure<ConnectionStringOptions>(Configuration.GetSection("ConnectionStrings"));
            // registro anche la classe che contiene le opzioni per 'Courses', definite in appsettings.json nella sezione 'Courses' 
            services.Configure<CoursesOptions>(Configuration.GetSection("Courses"));
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
