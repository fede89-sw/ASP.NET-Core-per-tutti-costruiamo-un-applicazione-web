using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyCourse.Models.Entities;
using MyCourse.Models.InputModels;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class EFCoreCourseService : ICourseService
    {
        private readonly MyCourseDbContext dbContext;
        public IOptionsMonitor<CoursesOptions> CoursesOptions { get; }
        public EFCoreCourseService(MyCourseDbContext dbContext, IOptionsMonitor<CoursesOptions> coursesOptions)
        {
            this.CoursesOptions = coursesOptions;
            this.dbContext = dbContext;
        }
        public async Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            CourseDetailViewModel courseDetail = await dbContext.Courses
                .AsNoTracking()
                .Where(course => course.Id == id)
                .Select(course => new CourseDetailViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImagePath = course.ImagePath,
                    Author = course.Author,
                    Rating = course.Rating,
                    FullPrice = course.FullPrice,
                    CurrentPrice = course.CurrentPrice,
                    Lessons = course.Lessons.Select(lesson => new LessonViewModel
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        Description = lesson.Description,
                        Duration = lesson.Duration
                    }).ToList()
                })
                    .SingleAsync();
            return courseDetail;
        }

        public async Task<List<CourseViewModel>> getCoursesAsync(CourseListInputModel model)
        {
            // page = Math.Max(1, page); // controllo nel caso l'utente smanetti e scriva pagina 0 o negativa. Cosi invece il minimo valore è 1, o la pagina passate se questa è > 1
            // int limit = CoursesOptions.CurrentValue.PerPage; // numero di oggetti dal database da recuperare(paginazione). Prendo il valore dai settings.json
            // int offset = (page - 1) * limit; // numero di oggetti da non recuperare prima di prendere i 10 oggetti (se sei a pagina 3, i primi 20 corsi non li vuoi)

            // se il search a sinistra di ?? è null viene restituito cio che è a dx dei ??, se no viene restituito search stesso.
            // Questo perchè se search è null, nel caso non venga fatta una ricerca per titolo, non mi torna neanche la lista di tutti i corsi,
            // visto che uso lo stesso metodo
            // search = search ?? ""; // Null Coalescing Operator

            // sanitizzo i dati di ordinamento in modo che sia uno di quello consentiti in appsettings.json
            // var orderOptions = CoursesOptions.CurrentValue.Order;
            // if(!orderOptions.Allow.Contains(orderby))
            // {
            //     orderby = orderOptions.By;
            //     ascending = orderOptions.Ascending;
            // }

            IQueryable<Course> baseQuery = dbContext.Courses;
            // uso l'extension method appropriato in base alla richiesta di ordinamento dell'utente
            switch(model.OrderBy)
            {
                case "Title":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Title);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Title);
                    }
                    break;
                case "Rating":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Rating);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Rating);
                    }
                    break;
                case "CurrentPrice":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.CurrentPrice.Amount);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.CurrentPrice.Amount);
                    }
                    break;
            }

            // prendo dal database usando la baseQuery con l'ordinamento scelto
            IQueryable<CourseViewModel> queriLinq = baseQuery
                .Where(course => course.Title.Contains(model.Search))
                .AsNoTracking()
                .Select(course => new CourseViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    ImagePath = course.ImagePath,
                    Author = course.Author,
                    Rating = course.Rating,
                    FullPrice = course.FullPrice,
                    CurrentPrice = course.CurrentPrice
                });

            List<CourseViewModel> courses = await queriLinq
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToListAsync();
            
            return courses;
        }
    }
}