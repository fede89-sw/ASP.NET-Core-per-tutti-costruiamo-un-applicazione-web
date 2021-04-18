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

        public async Task<List<CourseViewModel>> getCoursesAsync()
        {
            IQueryable<CourseViewModel> queriLinq = dbContext.Courses
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