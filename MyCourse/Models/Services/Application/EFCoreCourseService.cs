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
        private readonly MyCourseDbContext dbContext;
        public EFCoreCourseService(MyCourseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            CourseDetailViewModel courseDetail = await dbContext.Courses
                .AsNoTracking()
                .Where(course => course.Id == id)
                .Select(course => new CourseDetailViewModel{
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImagePath = course.ImagePath,
                    Author = course.Author,
                    Rating = course.Rating,
                    FullPrice = course.FullPrice,
                    CurrentPrice = course.CurrentPrice,
                    Lessons = course.Lessons.Select(lesson => new LessonViewModel{
                            Id = lesson.Id,
                            Title = lesson.Title,
                            Description = lesson.Description,
                            Duration = lesson.Duration
                        }).ToList() 
                    })             
                    .SingleAsync(); 
            return courseDetail;
        }

        public async Task<List<CourseViewModel>> getCoursesAsync(string search)
        {
            // se il search a sinistra di ?? è null viene restituito cio che è a dx dei ??, se no viene restituito search stesso.
            // Questo perchè se search è null, nel caso non venga fatta una ricerca per titolo, non mi torna neanche la lista di tutti i corsi,
            // visto che uso lo stesso metodo
            search = search ?? ""; // Null Coalescing Operator
            IQueryable<CourseViewModel> queriLinq = dbContext.Courses
            .Where(course => course.Title.Contains(search))
                .AsNoTracking()
                .Select(course => new CourseViewModel{
                    Id = course.Id,
                    Title = course.Title,
                    ImagePath = course.ImagePath,
                    Author = course.Author,
                    Rating = course.Rating,
                    FullPrice = course.FullPrice,
                    CurrentPrice = course.CurrentPrice
                });

            List<CourseViewModel> courses = await queriLinq.ToListAsync();
            return courses;
        }
    }
}