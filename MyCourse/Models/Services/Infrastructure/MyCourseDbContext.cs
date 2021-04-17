using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyCourse.Models.Entities
{
    public partial class MyCourseDbContext : DbContext
    {
        public MyCourseDbContext()
        {
        }

        public MyCourseDbContext(DbContextOptions<MyCourseDbContext> options)
            : base(options)
        {
        }

        // proprietà DbSet<>
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // warning To protect potentially sensitive information in your connection string, 
                // you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 
                // for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=Data/MyCourse.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // versione corrente di ASP.NET Core
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            // metodo per indicare il mapping, ovvero come ciascuna delle classi di entità
            // deve mappare sulla rispettiva tabella 
            modelBuilder.Entity<Course>(entity =>
            {
                // indico su che tabella rimappa la nostra classe di entità;
                // se però il nome della tabella ha lo stesso nome della proprietà del nome 
                // che esprime il DbSet( DbSet<Courses> Courses), è superfluo indicarlo perchè per convenzione lo sa da se
                entity.ToTable("Courses"); 

                //indichiamo la proprietà in classe 'Courses' che indica la PK
                entity.HasKey(course => course.Id); // superfluo se la proprietà si chiama Id o CoursesId
                // entity.HasKey(course => new {course.Id, course.Author}); // se la PK fosse composta da più campi la specifichi cosi

                // finchè il nome delle proprietà della classe sono = al nome della colonna del DB posso anche non specificare il campo

                //la region mi permette di creare un area che posso comprimere come visualizzazione in VSCode
                #region mapping generato automaticamente dal tool di reverse engineering (DatabaseFirst) 
            //     // metodo 'Entity<Courses>' configura l'entità 'Courses'
            //     entity.Property(e => e.Id).ValueGeneratedNever();

            //     entity.Property(e => e.Author)
            //         .IsRequired()
            //         .HasColumnType("TEXT (100)");

            //     entity.Property(e => e.CurrentPriceAmount)
            //         .IsRequired()
            //         .HasColumnName("CurrentPrice_Amount")
            //         .HasColumnType("NUMERIC")
            //         .HasDefaultValueSql("0");

            //     entity.Property(e => e.CurrentPriceCurrency)
            //         .IsRequired()
            //         .HasColumnName("CurrentPrice_Currency")
            //         .HasColumnType("TEXT (3)")
            //         .HasDefaultValueSql("'EUR'");

            //     entity.Property(e => e.Description).HasColumnType("TEXT (8000)");

            //     entity.Property(e => e.Email).HasColumnType("TEXT (100)");

            //     entity.Property(e => e.FullPriceAmount)
            //         .IsRequired()
            //         .HasColumnName("FullPrice_Amount")
            //         .HasColumnType("NUMERIC")
            //         .HasDefaultValueSql("0");

            //     entity.Property(e => e.FullPriceCurrency)
            //         .IsRequired()
            //         .HasColumnName("FullPrice_Currency")
            //         .HasColumnType("TEXT (3)")
            //         .HasDefaultValueSql("'EUR'");

            //     entity.Property(e => e.ImagePath).HasColumnType("TEXT (100)");

            //     entity.Property(e => e.Title)
            //         .IsRequired()
            //         .HasColumnType("TEXT (100)");
                    #endregion
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                #region mapping generato automaticamente dal tool di reverse engineering (DatabaseFirst)
                // // metodo 'Entity<Lessons>' configura l'entità 'Lessons'
                // entity.Property(e => e.Id).ValueGeneratedNever();

                // entity.Property(e => e.Description).HasColumnType("TEXT (8000)");

                // entity.Property(e => e.Duration)
                //     .IsRequired()
                //     .HasColumnType("TEXT (8)")
                //     .HasDefaultValueSql("'00:00:00'");

                // entity.Property(e => e.Title)
                //     .IsRequired()
                //     .HasColumnType("TEXT (100)");

                // entity.HasOne(d => d.Course)
                //     .WithMany(p => p.Lessons)
                //     .HasForeignKey(d => d.CourseId);
                #endregion
            });
        }
    }
}
