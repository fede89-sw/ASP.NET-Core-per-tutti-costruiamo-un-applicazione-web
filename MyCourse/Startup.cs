using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace MyCourse
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // devo aggiungere questo 'service' per usare il 'routing';
            // SetCompatibilityVersion è perchè ho aggiornato l'app a 2.2= dico ai 'services' di comportarsi secondo le tecniche introdotte nella versione 2.2
            // (dopo averla installata e aggiornata nella app, cambiando i riferimenti nei file che specificano la versione di dotnet)
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
