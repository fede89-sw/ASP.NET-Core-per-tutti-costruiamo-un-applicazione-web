using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.InputModels;

namespace MyCourse.Custom.ModelBinder
{
    public class CourseListInputModelBinder : IModelBinder
    {
        private readonly IOptionsMonitor<CoursesOptions> courseOptions;
        public CourseListInputModelBinder(IOptionsMonitor<CoursesOptions> courseOptions)
        {
            this.courseOptions = courseOptions;

        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //Recuperiamo i valori grazie ai value provider
            string search = bindingContext.ValueProvider.GetValue("Search").FirstValue;
            string orderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
            int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
            bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);

            //Creiamo l'istanza del CourseListInputModel
            var inputModel = new CourseListInputModel(search, page, orderBy, ascending, courseOptions.CurrentValue.PerPage, courseOptions.CurrentValue);

            //Impostiamo il risultato per notificare che la creazione è avvenuta con successo
            bindingContext.Result = ModelBindingResult.Success(inputModel);

            //Restituiamo un task completato
            return Task.CompletedTask;
        }
    }
}