using Microsoft.EntityFrameworkCore;
using MyCourse.Models.Entities;

namespace ef.Entities
{
    public class MyCourseContext : DbContext
    {
        // DbContext necessario per usare EF
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // setto la 'connection string' per il database
            optionsBuilder.UseSqlite("Filename=Data/EF_CF_example.db");
        }
        
        // Riferimenti alle enitità che creo 
        public DbSet<Courses> Courses { get; set; }
    }
}