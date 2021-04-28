using System.Collections.Generic;
using MyCourse.Models.InputModels;

namespace MyCourse.Models.ViewModels
{
    // Creo questa classe per passare alla View che visualizza l'elenco dei corsi, oltre la lista di questi
    // anche gli input inseriti dall'utente, in modo da visualizzarli anche nella pagina di ricerca e/o
    // per mantenere la preferenza di ordinamento 
    public class CourseListViewModel
    {
        public ListViewModel<CourseViewModel> Courses { get; set; }
        public CourseListInputModel Input { get; set; }
    }
}