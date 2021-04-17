// Classe con il modello per 'Courses' che verrà rapprensentato da una tabella nel database
namespace MyCourse.Models.Entities
{
    public partial class Courses
    {

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
 
    }
}
