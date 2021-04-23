using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Exceptions;

namespace MyCourse.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            // IExceptionHandlerPathFeature fornita dall' UseExceptionHandler; alla seconda richiesta
            // HTTP aggiunge al Context informazioni sull'errore della prima richiesta
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            // feature ha come proprietà 'Error' che contiene l'errore e 'Path' che contiene 
            // l'indirizzo URL che l'ha generata

            switch(feature.Error) 
            {
                // case InvalidOperationException err: // questo errore può essere sollevato anche da altre circostanze;
                case CourseNotFoundException err: // creo una classe derivata da Exception specifica per questo errore
                    ViewData["Title"] = "Corso non Trovato!";
                    Response.StatusCode = 404; // Setto lo status code 404 
                    return View("CourseNotFound"); // restituisco la view CourseNotFound

                default:
                    // errore generico
                    ViewData["Title"] = "Errore";
                    return View();
            }          
        }
    }
}