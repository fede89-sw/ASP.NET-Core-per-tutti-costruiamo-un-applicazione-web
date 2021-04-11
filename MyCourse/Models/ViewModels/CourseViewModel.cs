using MyCourse.Models.ValueTypes;

namespace MyCourse.Models.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }

        // creo classe 'Money' per i prezzi, cosi da implementare la possibilità di multi-valute,
        // ovvero la possiblità per i docenti di vendere il corso il EUR, USD, GBP
        public Money FullPrice { get; set; }
        public Money CurrentPrice { get; set; }  
    }
}