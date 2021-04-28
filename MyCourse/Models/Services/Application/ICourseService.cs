using System.Collections.Generic;
using System.Threading.Tasks;
using MyCourse.Models.InputModels;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public interface ICourseService
    {
        Task<ListViewModel<CourseViewModel>> getCoursesAsync(CourseListInputModel model);
        Task<CourseDetailViewModel> getCourseDetailAsync(int id);
    }
}