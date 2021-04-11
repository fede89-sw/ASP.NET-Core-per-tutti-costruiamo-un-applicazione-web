using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc; // per derivare da classe 'controller'
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers // per convezione nome del progetto e cartella in cui si trova il file
{
    public class CoursesController : Controller // derivo da classe base 'Controllers'
    {
        // Metodi Action
        public IActionResult Index(){
            // metodo Action per mostrare la lista di tutti i corsi
            // return Content("Sono il metodo Index"); // 'Content' restituisce una stringa statica

            var courseService = new CourseService(); // creo istanza di classe 'CourseService';
                                                     // uso var invece di fare (CourseService courseService = new CourseService();) 
                                                     // in quanto sto istanziando una classe con la keyword 'new' quindi il compilatore sa che l'oggetto 'courseService' è di quel tipo 
            List<CourseViewModel> courses_list = courseService.getCourses(); // courses_list è variabile di tipo 'List' di oggetti 'CourseViewModel'
            return View(courses_list); // ritorna la view che per convenzione deve essere 'Views/Courses/Index.cshtml'; passo la variabile 'courses_list'
        }
        public IActionResult Detail(int id) {
            // metodo per i dettagli di un singolo corso
            // return Content($"Sono il metodo Detail, ho ricevuto come parametro: {id}");
            return View();
        }
    }
}