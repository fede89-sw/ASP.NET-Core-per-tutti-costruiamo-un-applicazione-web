using System.Collections.Generic;

namespace MyCourse.Models.ViewModels
{
    // creo questa classe per poter avere e passare alla view, oltre il numero di corsi in base alla
    // paginazione, che il numero totale di corsi del database, in modo da generare dinamicamente i link
    // delle pagine
    public class ListViewModel<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}