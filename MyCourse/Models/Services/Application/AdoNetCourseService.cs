using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourse.Models.Exceptions;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ValueTypes;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {

        private readonly IDatabaseService db;
        public readonly IOptionsMonitor<CoursesOptions> CoursesOptions;
        public readonly ILogger<AdoNetCourseService> Logger;

        public AdoNetCourseService(IDatabaseService db, IOptionsMonitor<CoursesOptions> coursesOptions, ILogger<AdoNetCourseService> logger)
        {
            this.Logger = logger;
            this.CoursesOptions = coursesOptions;
            this.db = db;
        }

        public async Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            Logger.LogInformation("Course {id} requested", id);

            FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id};
                                         SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";

            DataSet dataSet = await db.QueryAsync(query);

            var courseTable = dataSet.Tables[0];
            if (courseTable.Rows.Count != 1)
            {
                Logger.LogWarning("Course {id} not Found", id);
                throw new CourseNotFoundException(id);
            }
            var courseRow = courseTable.Rows[0];
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            var lessonDataTable = dataSet.Tables[1];
            foreach (DataRow lessonRow in lessonDataTable.Rows)
            {
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                courseDetailViewModel.Lessons.Add(lessonViewModel);
            }
            return courseDetailViewModel;
        }

        public async Task<List<CourseViewModel>> getCoursesAsync(string search, int page, string orderby, bool ascending)
        {
            page = Math.Max(1, page); // controllo nel caso l'utente smanetti e scriva pagina 0 o negativa. Cosi invece il minimo valore è 1, o la pagina passate se questa è > 1
            int limit = CoursesOptions.CurrentValue.PerPage; // numero di oggetti dal database da recuperare(paginazione). Prendo il valore dai settings.json
            int offset = (page - 1) * limit; // numero di oggetti da non recuperare prima di prendere i 10 oggetti (se sei a pagina 3, i primi 20 corsi non li vuoi)
            
            // sanitizzo i dati di ordinamento in modo che sia uno di quello consentiti in appsettings.json
            var orderOptions = CoursesOptions.CurrentValue.Order;
            if(!orderOptions.Allow.Contains(orderby))
            {
                orderby = orderOptions.By;
                ascending = orderOptions.Ascending;
            }
            // se ordini per current price, essendoci valute diverse, ordino tutto in base al valore del prezzo, non interessandomi della valuta
            if(orderby == "CurrentPrice") {
                orderby = "CurrentPrice_Amount";
            }
            // trasformo il booleano 'ascending' in stringa 'ASC' o 'DESC' per usarla nella Query Sql
            string direction = ascending ? "ASC" : "DESC";

            // 'direction' e 'orderby' non devono essere convertiti in SqlParameters in quanto fanno parte integrante della Query, ovvero
            // formano 'ORDER BY Title DESC'. Completo questa funzionalità con un if in QueryAsync di DatabaseService.cs dove creo i SqlParameters

            // se search è null, ovvero non viene cercato un titolo, torna tutti i corsi in quanto la query
            // diventa un SELECT campi FROM courses dove il titolo contiene ""..condizione sempre vera
            FormattableString query = $"SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE title LIKE {'%' + search + '%'} ORDER BY {(Sql) orderby} {(Sql) direction} LIMIT {limit} OFFSET {offset}";
            DataSet query_result = await db.QueryAsync(query);

            var dataTable = query_result.Tables[0];
            var courseList = new List<CourseViewModel>();

            foreach (DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
        }
    }
}