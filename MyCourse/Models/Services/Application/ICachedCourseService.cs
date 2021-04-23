namespace MyCourse.Models.Services.Application
{
    public interface ICachedCourseService : ICourseService
    {
       // Ho gli stessi metodi di ICourseService ma la differenza che qui userò
       // la gestione della Cache, mentre in ICourseService ogni richiesta interpellerà il DB.
    }
}