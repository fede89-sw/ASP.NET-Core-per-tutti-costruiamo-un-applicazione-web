﻿using Microsoft.AspNetCore.Builder;
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
            services.AddResponseCaching();

            services.AddMvc( options =>
            {
                // setto le opzioni per il ResponseCache dell'home controller, prendendole dal file appsettings.json
                // e collegandone i valori come fatto nelle altre configurazione con IConfiguration
                var homeProfile = new CacheProfile();

                // homeProfile.Duration = Configuration.GetValue<int>("ResponseCache:Home:Duration");
                // homeProfile.Location = Configuration.GetValue<ResponseCacheLocation>("ResponseCache:Home:Location");
                // Visto che il nome Duration è lo stesso sia per homeProfile.Duration che ResponseCache:Home:Duration,
                // posso fare le 2 righe sopra di codice in quella qui sotto con Bind.
                Configuration.Bind("ResponseCache:Home", homeProfile);
                
                // Configurazione di ResponseCache per risolvere il problema del salvataggio in Cache da parte dell'app 
                // di View paginate (se no per lui salvare la homepage o la homepage?page=2 non cambia), in quanto nel creare
                // la chiave Cache non tiene conto dei query params; diciamogli di farci attenzione (al params 'page') come fatto qui sotto
                // homeProfile.VaryByQueryKeys = new string[] { "page" };
                // SETTO QUESTA CONFIGURAZIONE IN 'appsettings.json'

               
                options.CacheProfiles.Add("Home", homeProfile);
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Usiamo AdoNet o EF per l'accesso ai dati?
            services.AddTransient<ICourseService, AdoNetCourseService>();  
            // services.AddTransient<ICourseService, EFCoreCourseService>(); 

            services.AddTransient<IDatabaseService, DatabaseService>();

            services.AddTransient<ICachedCourseService, MemoryCacheCourseService>();

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

            // opzioni per la durata degli oggetti in Cache
            services.Configure<CachedLifeOptions>(Configuration.GetSection("CachedLife"));

            // opzioni per l'uso massimo di memoria RAM tramite Cache; Il nome in GetSection lo decido io
            // come al solito in base al nome messo nella configurazione(per es. in appsettings.json);
            // MemoryCacheOptions è una classe già presente in ASP.NET Core, non è creata da me.
            services.Configure<MemoryCacheOptions>(Configuration.GetSection("MemoryCache"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                // se ambiente non Development, uso questo Middleware per mostrare gli errori dovuti ad Eccezioni;
                // Con questo metodo l'indirizzo URL che ha generato un'eccezione viene rielaborata e provata una seconda volta,
                // usando però come URL quello che abbiamo passato noi, /error; in qst modo invece di mostrare un errore di server o senza info
                // verrà mostrata una pagina apposita e più User Friendly con info sull'errore.
                // Andremo a creare nella cartella Views una sottocartella Error con Index.cs, gestita da un ErrorController
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
