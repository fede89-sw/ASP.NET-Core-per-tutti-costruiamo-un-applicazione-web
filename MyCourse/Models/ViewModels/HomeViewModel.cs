using System.Collections.Generic;

namespace MyCourse.Models.ViewModels
{
    public class HomeViewModel : CourseViewModel
    {
        public List<CourseViewModel> LatestCourses { get; set; }
        public List<CourseViewModel> BestRatingCourses { get; set; }
    }
}