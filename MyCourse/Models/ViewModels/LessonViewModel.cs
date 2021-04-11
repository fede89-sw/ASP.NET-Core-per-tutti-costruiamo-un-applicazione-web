using System;

namespace MyCourse.Models.ViewModels
{
    public class LessonViewModel
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        // TimeSpan in C# è il tipo che identifica le durate temporali
    }
}