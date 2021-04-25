using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    // l'attributo avrebbe effetto su tutte le action del controller
    // [ResponseCache(CacheProfileName = "Home")] 
    public class HomeController : Controller
    {
        [ResponseCache(CacheProfileName = "Home")]
        public IActionResult Index() {
            ViewData["Title"] = "MyCourse - Homepage";
            return View();
        }
    }
}