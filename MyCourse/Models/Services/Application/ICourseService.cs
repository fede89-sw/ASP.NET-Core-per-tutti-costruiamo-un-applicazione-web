// Interfaccia; non contiene alcuna logica ma contiene un elenco di proprietà, metodi ed eventi
using System.Collections.Generic;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public interface ICourseService
    {
        // non uso 'public' perchè omettendolo è implicito
        // public List<CourseViewModel> getCourses(); = List<CourseViewModel> getCourses();
        List<CourseViewModel> getCourses();
        CourseDetailViewModel getCourseDetail(int id);
    }
}