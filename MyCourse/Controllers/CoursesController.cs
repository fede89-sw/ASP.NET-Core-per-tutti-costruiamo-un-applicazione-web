using Microsoft.AspNetCore.Mvc; // per derivare da classe 'controller'

namespace MyCourse.Controllers // per convezione nome del progetto e cartella in cui si trova il file
{
    public class CoursesController : Controller // derivo da classe base 'Controllers'
    {
        // Metodi Action
        public IActionResult Index(){
            // metodo Action per mostrare la lista di tutti i corsi
            // return Content("Sono il metodo Index"); // 'Content' restituisce una stringa statica
            return View(); // ritorna la view che per convenzione deve essere 'Views/Courses/Index.cshtml'
        }
        public IActionResult Detail(int id) {
            // metodo per i dettagli di un singolo corso
            // return Content($"Sono il metodo Detail, ho ricevuto come parametro: {id}");
            return View();
        }
    }
}