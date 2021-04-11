using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc; // per derivare da classe 'controller'
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers // per convezione nome del progetto e cartella in cui si trova il file
{
    public class CoursesController : Controller // derivo da classe base 'Controllers'
    {
        // Costruttore ( ctor + TAB)
        private readonly CourseService courseService;
        public CoursesController(CourseService courseService) // (CTRL + . su 'courseService', 'initialize field from parameter')
        {
            this.courseService = courseService;

        }
        // Metodi Action
        public IActionResult Index()
        {
            // metodo Action per mostrare la lista di tutti i corsi
            // return Content("Sono il metodo Index"); // 'Content' restituisce una stringa statica

            List<CourseViewModel> courses_list = courseService.getCourses(); // courses_list è variabile di tipo 'List' di oggetti 'CourseViewModel'
            ViewData["Title"] = "MyCourse - Catalogo dei Corsi";
            return View(courses_list); // ritorna la view che per convenzione deve essere 'Views/Courses/Index.cshtml'; passo la variabile 'courses_list'
        }
        public IActionResult Detail(int id)
        {
            // metodo per i dettagli di un singolo corso
            // return Content($"Sono il metodo Detail, ho ricevuto come parametro: {id}");

            CourseDetailViewModel course_detail = courseService.getCourseDetail(id);
            // 'CourseDetailViewModel' nome scelto da noi. Creeremo il file 'CourseDetailModel.cs' in 'ViewModel' che definisce l'oggetto
            // 'CourseDetailViewModel'. Esso è formato da tutte le caratteristiche definite in 'CourseViewModel'.
            // Andremo infatti a derivare da quella classe per non ripetere il codice 
            ViewData["Title"] = "MyCourse - " + course_detail.Title;
            return View(course_detail);
        }
    }
}