using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class EFCoreCourseService : ICourseService
    {
        // rendo 'EFCoreCourseService' dipendente da MyCourseDbContext, in quanto ho bisogno di lui 
        // per accedere poi al Database
        private readonly MyCourseDbContext dbContext;
        // Dipendo dall'implementazione e non da un'interfaccia perchè farlo con il dbContext
        // semplicemente è un casino. Quindi in questi casi va bene fare cosi, non tiriamoci matti
        // per dover per forza inseguire un'idea
        public EFCoreCourseService(MyCourseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<CourseViewModel>> getCoursesAsync()
        {
            // dobbiamo ottenere delle istanze di 'CourseViewModel' a partire da enitità di tipo 'Course';
            // Usiamo quindi 'Select' di Linq, creiamo un istanza di CourseViewModel in cui mappiamo 
            // i valori ottenuti dal dbContext
            // List<CourseViewModel> courses = dbContext.Courses;

            List<CourseViewModel> courses = await dbContext.Courses.Select(course => new CourseViewModel{
                Id = course.Id,
                Title = course.Title,
                ImagePath = course.ImagePath,
                Author = course.Author,
                Rating = course.Rating,
                FullPrice = course.FullPrice,
                CurrentPrice = course.CurrentPrice
            }).ToListAsync(); // rendo il risultato di 'Select', che è IQueryable in una List
            return courses;
        }
    }
}