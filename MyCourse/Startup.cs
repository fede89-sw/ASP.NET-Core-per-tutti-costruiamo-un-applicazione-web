using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MyCourse
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        // ConfigureServices serve tenere una sorta di registro dei nostri servizi, in cui andiamo a legare
        // delle Interfacce ad implementazioni concrete (eseguite dalle Classi)
        public void ConfigureServices(IServiceCollection services)
        {
            // devo aggiungere questo 'service' per usare il 'routing';
            // SetCompatibilityVersion è perchè ho aggiornato l'app a 2.2= dico ai 'services' di comportarsi secondo le tecniche introdotte nella versione 2.2
            // (dopo averla installata e aggiornata nella app, cambiando i riferimenti nei file che specificano la versione di dotnet)
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddTransient<ICourseService, EFCoreCourseService>(); 
            // services.AddTransient<ICourseService, AdoNetCourseService>(); 
            // diciamo ad ASP.NET Core che ogni volta che incontra un componente, come il nostro CourseController che ha una dipendenza 
            // dall'interfaccia ICourseService, in realtà construisci un CurseService(in qnt non può creare un istanza di ICourseService, non 
            // essendo una classe ma un'interfaccia); In qst modo abbiamo reso i componenti debolmente accoppiati, non dipendendo più direttamente
            //  da CourseService, che può essere sostituito cn qualsiasi classe che implementi i metodi definiti in ICourseService).
            
            services.AddTransient<IDatabaseService, DatabaseService>();

            // aggiungo il Context; Scoped perchè è oneroso da creare, (fa il mapping e contiene configurazione),
            // quindi faccio si di avere al massimo una istanza per richiesta HTTP;
            // services.AddScoped<MyCourseDbContext>();
            // Microsoft ha però creato una classe apposita per il Context, che è si scoped ma fa anche un servizio di logging, ovvero
            // ogni volta che andiamo a leggere i risultati di una query Linq viene loggata sul terminale la query SQL eseguita
            // services.AddDbContext<MyCourseDbContext>();
            // ottimizzazione per precaricare istanze di MyCourseDbContext nel DbContext Pool;
            // devi fornire una configurazione passando un DbContextOptionsBuilder e opzionalmente il numero
            // di DbContext da precaricare (poolSize), che di default è 128
            services.AddDbContextPool<MyCourseDbContext>(optionBuilder => {
                // warning To protect potentially sensitive information in your connection string, 
                // you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 
                // for guidance on storing connection strings.
                optionBuilder.UseSqlite("Data Source=Data/MyCourse.db");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });

            // Middleware di Routing
            // app.UseMvcWithDefaultRoute();
            app.UseMvc( routeBuilder => { // lambda expression
                // definisco una 'Route' a cui passo il nome e un template; Praticamente definisco la Route come faccio
                // in Django nel file urls.py; il percorso è il template, il nome della route è 'default'. 
                // ES. courses/detail/5 -> controller=courses, action=detail, parametro di action=5 (Queste informazioni sono presenti in 'Route Data')
                routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); 
                // posso settare dei valori di default dell'indirizzo facendo:
                /*  
                    {controller=Home} -> controller di default = Home
                    {action=Index} -> action di default = Index
                    {id?} -> id è un parametro opzionale
                */
                // In questo modo anche se non do nell'url le info necessarie ho cmq un indirizzo di default(che corrisponde alla homepage)
            });
        }
    }
}
