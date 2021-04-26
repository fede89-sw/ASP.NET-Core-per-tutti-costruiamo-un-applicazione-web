using System.Collections.Generic;
using System.Threading.Tasks;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public interface ICourseService
    {
        Task<List<CourseViewModel>> getCoursesAsync(string search, int page);
        Task<CourseDetailViewModel> getCourseDetailAsync(int id);
    }
}