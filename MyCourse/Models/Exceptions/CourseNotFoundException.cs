using System;

namespace MyCourse.Models.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        public CourseNotFoundException(int courseId) : base($"Course {courseId} Not Found")
        {
            
        }
    }
}