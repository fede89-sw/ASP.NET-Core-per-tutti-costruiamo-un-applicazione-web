using System;
using System.Collections.Generic;
using MyCourse.Models.ValueTypes;

namespace MyCourse.Models.Entities
{
    public partial class Course
    {
        public Course(string title, string author)
        {
            // validazione campi passati al costruttore
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("The course must have a title");
            }
			if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("The course must have an author");
            }   
            // voglio che per ogni istanza di Course siano inseriti title e author
            Title = title;
            Author = author;
            Lessons = new HashSet<Lesson>();
        }

        // rendo privato il set dei dati, cosi che solo con un metodo apposito posso salvarci i dati,
        // posso cosi inserire delle validazioni ai dati
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Author { get; private set; }
        public string Email { get; private set; }
        public double Rating { get; private set; }
        public Money FullPrice { get; private set; }
        public Money CurrentPrice { get; private set; }


        public void ChangeTitle(string newTitle)
        {
            // validazione dati prima di cambiarli
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("The course must have a title");
            }
            Title = newTitle;
        }

        public void ChangePrices(Money newFullPrice, Money newDiscountPrice)
        {
            // validazione dati prima di cambiarli
            if (newFullPrice == null || newDiscountPrice == null)
            {
                throw new ArgumentException("Prices can't be null");
            }
            if (newFullPrice.Currency != newDiscountPrice.Currency)
            {
                throw new ArgumentException("Currencies don't match");
            }
            if (newFullPrice.Amount < newDiscountPrice.Amount)
            {
                throw new ArgumentException("Full price can't be less than the current price");
            }
            FullPrice = newFullPrice;
            CurrentPrice = newDiscountPrice;
        }

        // rapporto di uno a molti tra il corso e le sue Lessons; il tipo è ICollection<T>
        public virtual ICollection<Lesson> Lessons { get; private set; }
    }
}
