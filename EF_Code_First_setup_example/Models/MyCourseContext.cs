using Microsoft.EntityFrameworkCore;
using MyCourse.Models.Entities;

namespace ef.Entities
{
    public class MyCourseContext : DbContext
    {
        // Posso impostare le opzioni del DbContext qui sotto oppure nel file Startup.cs
        // in ConfigureService; se lo faccio in Startup.cs devo usare il costruttore qui sotto,
        // altrimenti posso non usarlo e invece usare la classe qui sotto 'OnConfiguring'
        
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     // setto la 'connection string' per il database
        //     optionsBuilder.UseSqlite("Filename=Data/EF_CF_example.db");
        // }
        
        public MyCourseContext(DbContextOptions<MyCourseContext> options) : base(options)
        {
        }

        // Riferimenti alle enitità che creo 
        public DbSet<Courses> Courses { get; set; }
    }
}