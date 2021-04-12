using System.Collections.Generic;
using System.Data;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {

        private readonly IDatabaseService db;

        public AdoNetCourseService(IDatabaseService db)
        {
            // rendo dipendente AdoNetCourseService da IDatabaseService,
            // il servizio infrastrutturale che sa come accedere al DB.
            this.db = db;
            
        }

        CourseDetailViewModel ICourseService.getCourseDetail(int id)
        {
            throw new System.NotImplementedException();
        }

        List<CourseViewModel> ICourseService.getCourses()
        {
            string query = "SELECT * FROM Courses";
            DataSet dataSet = db.Query(query); 
            throw new System.NotImplementedException();
        }
    }
}