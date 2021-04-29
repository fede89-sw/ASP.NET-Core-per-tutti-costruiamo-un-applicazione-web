using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    // l'attributo avrebbe effetto su tutte le action del controller
    // [ResponseCache(CacheProfileName = "Home")] 
    public class HomeController : Controller
    {
        private readonly ICachedCourseService courseService;
        public HomeController(ICachedCourseService courseService)
        {
            this.courseService = courseService;

        }

        [ResponseCache(CacheProfileName = "Home")]
        // public IActionResult Index([FromServices] ICachedCourseService courseService)
        public async Task<IActionResult> Index()
        {
            // per permettere al model Binding di fornirmi l'istanza di ICachedCourseService devo
            // specificare FromServices, ovvero di cercare la classe nei servizi registrati per la 
            // dependency injection 
            ViewData["Title"] = "MyCourse - Homepage";
            List<CourseViewModel> bestRatingCourses = await courseService.getBestRatingCoursesAsync();
            List<CourseViewModel> latestCourses = await courseService.getLatestCourses();

            HomeViewModel viewModel = new HomeViewModel {
                BestRatingCourses = bestRatingCourses,
                LatestCourses = latestCourses
            };

            return View(viewModel);
        }
    }
}