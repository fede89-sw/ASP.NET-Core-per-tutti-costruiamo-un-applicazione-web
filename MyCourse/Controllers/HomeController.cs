using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            ViewData["Title"] = "MyCourse - Homepage";
            return View();
        }
    }
}