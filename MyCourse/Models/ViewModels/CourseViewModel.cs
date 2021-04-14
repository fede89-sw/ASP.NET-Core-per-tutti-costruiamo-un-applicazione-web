using System;
using System.Data;
using MyCourse.Models.Enums;
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

        public static CourseViewModel FromDataRow(DataRow courseRow)
        {
            // creo istanza di CourseViewModel con i valori passati come parametro, per quando prendo i dati dal database
            // e li devo mappare nel ciclo foreach per poi mandarli alla razor page
            var courseViewModel = new CourseViewModel {
                Title = Convert.ToString(courseRow["Title"]), // oppure Title = (string) courseRow["Title"]
                ImagePath = Convert.ToString(courseRow["ImagePath"]),
                Author = Convert.ToString(courseRow["Author"]),
                Rating = Convert.ToDouble(courseRow["Rating"]),
                FullPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                    Convert.ToDecimal(courseRow["FullPrice_Amount"])
                ),
                CurrentPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                    Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                ),
                Id = Convert.ToInt32(courseRow["Id"])
            };
            return courseViewModel;
        }
    }
}