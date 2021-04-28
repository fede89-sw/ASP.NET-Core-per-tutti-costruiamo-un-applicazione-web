using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourse.Models.Exceptions;
using MyCourse.Models.InputModels;
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

        public async Task<ListViewModel<CourseViewModel>> getCoursesAsync(CourseListInputModel model)
        {
            // page = Math.Max(1, page); // controllo nel caso l'utente smanetti e scriva pagina 0 o negativa. Cosi invece il minimo valore è 1, o la pagina passate se questa è > 1
            // int limit = CoursesOptions.CurrentValue.PerPage; // numero di oggetti dal database da recuperare(paginazione). Prendo il valore dai settings.json
            // int offset = (page - 1) * limit; // numero di oggetti da non recuperare prima di prendere i 10 oggetti (se sei a pagina 3, i primi 20 corsi non li vuoi)
                        // se ordini per current price, essendoci valute diverse, ordino tutto in base al valore del prezzo, non interessandomi della valuta
            // if(orderby == "CurrentPrice") {
            //     orderby = "CurrentPrice_Amount";
            // }
            
            string orderby = model.OrderBy == "CurrentPrice" ? "CurrentPrice_Amount" : model.OrderBy;
            // trasformo il booleano 'ascending' in stringa 'ASC' o 'DESC' per usarla nella Query Sql
            string direction = model.Ascending ? "ASC" : "DESC";

            // 'direction' e 'orderby' non devono essere convertiti in SqlParameters in quanto fanno parte integrante della Query, ovvero
            // formano 'ORDER BY Title DESC'. Completo questa funzionalità con un if in QueryAsync di DatabaseService.cs dove creo i SqlParameters

            // se search è null, ovvero non viene cercato un titolo, torna tutti i corsi in quanto la query
            // diventa un SELECT campi FROM courses dove il titolo contiene ""..condizione sempre vera
            FormattableString query = $@"SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE title LIKE {'%' + model.Search + '%'} ORDER BY {(Sql) orderby} {(Sql) direction} LIMIT {model.Limit} OFFSET {model.Offset};
            SELECT COUNT(*) FROM Courses WHERE title LIKE {'%' + model.Search + '%'}";
            
            DataSet query_result = await db.QueryAsync(query);

            var dataTable = query_result.Tables[0];
            var courseList = new List<CourseViewModel>();

            foreach (DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }

            ListViewModel<CourseViewModel> result = new ListViewModel<CourseViewModel>{
                Results = courseList,
                TotalCount = Convert.ToInt32(query_result.Tables[1].Rows[0][0]) // prendo il conteggio dei corsi totali fatto con la seconda Query Sql inviata
            };
            return result;
        }
    }
}