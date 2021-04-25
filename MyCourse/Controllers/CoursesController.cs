using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller 
    {
        private readonly ICachedCourseService courseService;
        public CoursesController(ICachedCourseService courseService) 
        {
            this.courseService = courseService;

        }
        public async Task<IActionResult> Index(string search=null, int page=1, string orderby="price", bool ascending=true)
        {
            List<CourseViewModel> courses_list = await courseService.getCoursesAsync(search);
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