using System;
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
            // eseguo 2 query (SqliteCommando posso farne più contemporaneamente ) che ritorneranno 2 DataTable presenti in in DataSet;
            // il simbolo '$' prima della stringa è come la f-string in python(uguali a come si fannoi in javascript);
            // il simbolo '@' vuol dire che stiamo definendo una stringa su più righe; cioè una stringa composta da più righe diverse(come 2 istruzioni diverse)
            FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id};
                                         SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";
            // la FormattableString è una stringa che permette di tenere separati la parte statica, di sola stringa e i paramentri
            // che la compongono; query.Format da la parte statica, query.GetArgumets() ritorna i parametri;

            // Chiamo metodo per eseguire la query; sarà un DataSet formato da 2 DataTable, perchè ho eseguito 2 query
            DataSet dataSet = db.Query(query);

            // Tables[0] -> primo DataTable dei risultati della prima query, del primo SELECT (Singolo Corso)
            var courseTable = dataSet.Tables[0];
            if (courseTable.Rows.Count != 1) {
                // se le righe sono diverse da uno sollevo eccezzione(un corso trovato nel Database tornerà per forza una sola riga)
                throw new InvalidOperationException($"Did not return exactly 1 row for Course {id}");
            }
            var courseRow = courseTable.Rows[0]; // leggo la riga della table
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow); //mappatura che mi torna oggetto CourseDetailViewModel da un DataRow

            // Tables[1] -> primo DataTable dei risultati della seconda query, del secondo SELECT (lezioni del corso)
            var lessonDataTable = dataSet.Tables[1];

            foreach(DataRow lessonRow in lessonDataTable.Rows) {
                //ciclo le lezioni perchè ce ne possono essere molte per un solo corso (relazioni uno a molti)
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow); //mappatura che mi torna oggetto LessonViewModel da un DataRow
                //aggiungo l'oggetto 'LessonViewModel' ovvero la singola lezione alla lista di lezioni del corso 
                courseDetailViewModel.Lessons.Add(lessonViewModel);
            }
            return courseDetailViewModel; 
        }

        List<CourseViewModel> ICourseService.getCourses()
        {
            FormattableString query = $"SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses";
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