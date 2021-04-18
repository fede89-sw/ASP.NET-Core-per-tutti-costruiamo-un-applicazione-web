using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {

        private readonly IDatabaseService db;

        public AdoNetCourseService(IDatabaseService db)
        {
            this.db = db;    
        }

        public async Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            // il simbolo '$' prima della stringa è come la f-string in python(uguali a come si fannoi in javascript);
            // il simbolo '@' vuol dire che stiamo definendo una stringa su più righe; cioè una stringa composta da più righe diverse(come 2 istruzioni diverse)
            FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id};
                                         SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";
            // la FormattableString è una stringa che permette di tenere separati la parte statica, di sola stringa e i paramentri
            // che la compongono; query.Format da la parte statica, query.GetArgumets() ritorna i parametri;

            DataSet dataSet = await db.QueryAsync(query);

            var courseTable = dataSet.Tables[0];
            if (courseTable.Rows.Count != 1) {
                throw new InvalidOperationException($"Did not return exactly 1 row for Course {id}");
            }
            var courseRow = courseTable.Rows[0];
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            var lessonDataTable = dataSet.Tables[1];
            foreach(DataRow lessonRow in lessonDataTable.Rows) {
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                courseDetailViewModel.Lessons.Add(lessonViewModel);
            }
            return courseDetailViewModel; 
        }

        public async Task<List<CourseViewModel>> getCoursesAsync()
        {
            FormattableString query = $"SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses";
            DataSet query_result = await db.QueryAsync(query);

            var dataTable = query_result.Tables[0];
            var courseList = new List<CourseViewModel>();

            foreach(DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
        }
    }
}