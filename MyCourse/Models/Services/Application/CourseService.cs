using System;
using System.Collections.Generic;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueTypes;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    // Classe che rappresente il nostro primo servizio applicativo
    public class CourseService
    {
        // la classe conterrà dei metodi che ci restituiranno i dati dei corsi in forma di oggetti ViewModel
        public List<CourseViewModel> getCourses() // metodo che torna la lista dei corsi, che sono oggetti 'CourseViewModel'
        {
            // crea istanze random di corsi e le aggiunge alla lista corsi 'courseList'
            var courseList = new List<CourseViewModel>();
            var rand = new Random();
            for (int i = 1; i <= 20; i++)
            {
                var price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
                var course = new CourseViewModel
                {
                    Id = i,
                    Title = $"Corso {i}",
                    CurrentPrice = new Money(Currency.EUR, price),
                    FullPrice = new Money(Currency.EUR, rand.NextDouble() > 0.5 ? price : price + 1),
                    Author = "Nome cognome",
                    Rating = rand.Next(10, 50) / 10.0,
                    ImagePath = "/town.jpg"
                };
                courseList.Add(course);
            }
            return courseList;
        }

        public CourseDetailViewModel getCourseDetail(int id)
        {
            var rand = new Random();
            var price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
            var course = new CourseDetailViewModel
            {
                Id = id,
                Title = $"Corso {id}",
                CurrentPrice = new Money(Currency.EUR, price),
                FullPrice = new Money(Currency.EUR, rand.NextDouble() > 0.5 ? price : price + 1),
                Author = "Nome cognome",
                Rating = rand.Next(10, 50) / 10.0,
                ImagePath = "/town.jpg",
                Description = $"Descrizione {id}",
                Lessons = new List<LessonViewModel>()
            };

            for (var i = 1; i <= 5; i++) {
                var lesson = new LessonViewModel {
                    Title = $"Lezione {i}",
                    Duration = TimeSpan.FromSeconds(rand.Next(40, 90))
                };
                course.Lessons.Add(lesson);
            }

            return course;
        }
    }
}