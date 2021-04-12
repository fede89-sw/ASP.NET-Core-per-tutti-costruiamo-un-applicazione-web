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
            // rendo dipendente, tramite il costruttore, AdoNetCourseService da IDatabaseService,
            // il servizio infrastrutturale che sa come accedere al DB, in quanto senza questo 
            // infatti non saprebbe come ricavare i dati dal db. Hai bisogno di lui per funzionare.
            this.db = db;
            
        }

        CourseDetailViewModel ICourseService.getCourseDetail(int id)
        {
            throw new System.NotImplementedException();
        }

        List<CourseViewModel> ICourseService.getCourses()
        {
            string query = "SELECT * FROM Courses";
            DataSet query_result = db.Query(query); 
            throw new System.NotImplementedException();
        }
    }
}