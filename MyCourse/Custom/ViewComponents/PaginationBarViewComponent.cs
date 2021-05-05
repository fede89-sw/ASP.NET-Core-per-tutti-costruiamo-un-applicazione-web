using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.ViewModels;

namespace MyCourse.Custom.ViewComponents
{
    public class PaginationBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CourseListViewModel model)
        {
            // numero di pagina corrente
            // numero di risultati totali
            // numero di risultati per pagina
            return View(model);
        }
    }
}