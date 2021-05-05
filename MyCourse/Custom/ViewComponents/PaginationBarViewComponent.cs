using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.ViewModels;

namespace MyCourse.Custom.ViewComponents
{
    public class PaginationBarViewComponent : ViewComponent
    {
        // public IViewComponentResult Invoke(CourseListViewModel model)
        // faccio dipendere il componente da un'interfaccia in modo che sia riutilizzabile per altre paginazioni,
        //  e non solo per oggetti di tipo 'CourseListViewModel'
        public IViewComponentResult Invoke(IPaginationInfo model)
        {
            // IL componente necessita di:
            // numero di pagina corrente
            // numero di risultati totali
            // numero di risultati per pagina
            // Search, OrderBy, Ascending
            return View(model);
        }
    }
}