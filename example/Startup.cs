using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace example
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction())
            {
                app.UseHttpsRedirection();  // renderizza tutte le richieste HTTP ad HTTPS, solo in produzione
            }
            app.UseStaticFiles();
            app.Run(async (context) =>
            {
                // await context.Response.WriteAsync("Ciao Mondo!");
                // https://miosito.it/products?id=1
                // context.Request.Host -> ottengo il dominio da cui sono stato chiamato nella barra indirizzi (miosito.it)
                // context.Request.Path -> ottengo la path corrente dalla barra indirizzi (/products)
                // context.Request.Query["id"] -> ottengo il valore di 'id' passato dalla barra indirizzi (?id=1)
                
                string name = context.Request.Query["name"];
                if(name != null && name != ""){
                    await context.Response.WriteAsync($"Ciao {name.ToUpper()}!");
                }
                else{
                    await context.Response.WriteAsync("Ciao Mondo!");
                }
            });
        }
    }
}
