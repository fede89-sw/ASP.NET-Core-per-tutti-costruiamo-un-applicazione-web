// Creo una classe per gestire le opzioni della connection string in maniera fortemente tipizzata;
// In questo modo la classe gestisce solo stringhe, come serve a me per la connectionString
namespace MyCourse.Models.Options
{
    public class ConnectionStringOptions
    {
        public string Default { get; set; }
    }
}