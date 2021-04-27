using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Custom.ModelBinder;
using MyCourse.Models.Options;

namespace MyCourse.Models.InputModels
{
    // Creo questa classe per compierci la sanitizzazione dei valori input passati dall'utente
    // tramite URL, invece che passarli al Controller cosi come sono.
    // Devo creare un mio ModelBinder in quanto se no mi da errore che non vengono passati i parametri
    // al costruttore quando lo chiamo
    [ModelBinder(BinderType = typeof(CourseListInputModelBinder))]
    public class CourseListInputModel
    {
        public CourseListInputModel(string search, int page, string orderby, bool ascending, CoursesOptions coursesOptions)
        {            
            // sanitizzo i dati di ordinamento in modo che sia uno di quello consentiti in appsettings.json
            var orderOptions = coursesOptions.Order;
            if (!orderOptions.Allow.Contains(orderby))
            {
                orderby = orderOptions.By;
                ascending = orderOptions.Ascending;
            }

            this.Ascending = ascending;
            this.OrderBy = orderby;
            this.Search = search ?? "";
            this.Page = Math.Max(1, Page);

            this.Limit = coursesOptions.PerPage;
            this.Offset = (this.Page - 1) * this.Limit;
        }

        // imposto solo 'get' perchè non voglio che dall'esterno i valori che sono stati sanitizzati
        // possano essere modificati. Solo questa classe può farlo.
        public string Search { get; }
        public int Page { get;}
        public string OrderBy { get; }
        public bool Ascending { get; }
        public int Limit { get; }
        public int Offset { get; }
    }
}