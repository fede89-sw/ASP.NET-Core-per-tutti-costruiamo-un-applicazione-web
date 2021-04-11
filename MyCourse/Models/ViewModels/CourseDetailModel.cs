using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCourse.Models.ViewModels
{
    public class CourseDetailViewModel : CourseViewModel
    {
        // aggiungo altre proprietà a quelle definite in 'CourseViewModel';
        // puoi aggiungere proprietà sfruttando Intellisense, usando l'abbreviazione 'prop' e poi TAB
        public string Description { get; set; }
        public List<LessonViewModel> Lessons { get; set; }

        public TimeSpan TotalCourseDuration
        {
            get => TimeSpan.FromSeconds(Lessons?.Sum(l => l.Duration.TotalSeconds) ?? 0);
        }
    }
}