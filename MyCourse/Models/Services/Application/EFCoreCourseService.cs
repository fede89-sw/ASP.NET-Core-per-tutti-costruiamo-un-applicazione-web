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
        public async Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            CourseDetailViewModel courseDetail = await dbContext.Courses
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
                        }).ToList() // 'Lessons' di CourseDetailViewModel è List<LessonViewModel>, quindi devo fare il mapping e renderlo 
                    })              // prima di tipo LessonViewModele poi una lista, perchè course.Lessons è ICollection<Lessons>
                    // Select torna un IQueryable anche se vi è un solo elemento, quindi devo ottenere un singolo oggetto, e lo faccio con 1 dei metodi qui sotto:
                    //.FirstOrDefaultAsync(); //Restituisce null se l'elenco è vuoto e non solleva mai un'eccezione
                    //.SingleOrDefaultAsync(); //Tollera il fatto che l'elenco sia vuoto e in quel caso restituisce null, oppure se l'elenco contiene più di 1 elemento, solleva un'eccezione
                    //.FirstAsync(); //Restituisce il primo elemento, ma se l'elenco è vuoto solleva un'eccezione
                    .SingleAsync(); // Restituisce il primo elemento di un elenco, ma se l'elenco ne contiene 0 o più solleva un eccezione
            return courseDetail;
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