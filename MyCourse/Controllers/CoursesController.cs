using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.InputModels;
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
        public async Task<IActionResult> Index(CourseListInputModel model)
        {
            List<CourseViewModel> courses_list = await courseService.getCoursesAsync(model);
            ViewData["Title"] = "MyCourse - Catalogo dei Corsi";
            
            // metto la lista dei corsi e le variabili passate dall'utente presenti in 'model'
            // nella classe 'CourseListViewModel', in modo da passare tutto alla View
            CourseListViewModel viewModel = new CourseListViewModel{
                Courses = courses_list,
                Input = model
            };
            
            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel course_detail = await courseService.getCourseDetailAsync(id);
            ViewData["Title"] = "MyCourse - " + course_detail.Title;
            return View(course_detail);
        }
    }
}