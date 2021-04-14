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
            string query = "SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses";
            DataSet query_result = db.Query(query);

            // prendo la tabella coi risultati presenti eseguendo la query
            var dataTable = query_result.Tables[0];

            // creo variabile per contenere la lista dei corsi, che verrà restituita poi alla Razor Page
            var courseList = new List<CourseViewModel>();

            // ciclo le righe della tabella coi risultati per salvarle i 'course' e aggiungere ogni corso alla lista dei corsi
            foreach(DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow); // faccio il mapping delle righe dei risultati
                courseList.Add(course);
            }
            return courseList;
        }
    }
}