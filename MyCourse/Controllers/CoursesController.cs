using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller 
    {
        private readonly ICourseService courseService;
        public CoursesController(ICourseService courseService) 
        {
            this.courseService = courseService;

        }
        public async Task<IActionResult> Index()
        {
            List<CourseViewModel> courses_list = await courseService.getCoursesAsync();
            ViewData["Title"] = "MyCourse - Catalogo dei Corsi";
            return View(courses_list);
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel course_detail = await courseService.getCourseDetailAsync(id);
            ViewData["Title"] = "MyCourse - " + course_detail.Title;
            return View(course_detail);
        }
    }
}