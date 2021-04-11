// Interfaccia; non contiene alcuna logica ma contiene un elenco di proprietà, metodi ed eventi
using System.Collections.Generic;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public interface ICourseService
    {
        List<CourseViewModel> getCourses();
        CourseDetailViewModel getCourseDetail(int id);
    }
}