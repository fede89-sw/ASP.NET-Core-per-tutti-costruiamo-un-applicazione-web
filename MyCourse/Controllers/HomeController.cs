using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    // l'attributo avrebbe effetto su tutte le action del controller
    // [ResponseCache(CacheProfileName = "Home")] 
    public class HomeController : Controller
    {
        // Decoro la View cosi da dichiarare che l'output HTML prodotto da questa View può essere messa
        // in Cache, in qst caso per 60 secondi
        // [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] 

        // definisco il profilo in appsettings.json e poi lo collego in ConfigureService di Startup.cs, 
        // in modo da poter riusare questo profilo "Home" con queste impostazioni o per modificarle più
        // facilmente dal file json.
        [ResponseCache(CacheProfileName = "Home")]
        public IActionResult Index() {
            ViewData["Title"] = "MyCourse - Homepage";
            return View();
        }
    }
}