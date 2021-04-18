using Microsoft.EntityFrameworkCore;
using MyCourse.Models.Entities;

namespace MyCourse.Models.Services.Infrastructure
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
                
                #region Mapping degli OwnedType
                /*
                Mapping di un OwnedType, ovvero di 'FullPrice' e 'CurrentPrice', che sono classi di tipo 'Money' contenenti le proprietà
                'Amount' e 'Currency'; OwnedType sono cioè classi che ci aiutano a tenere coesi più valori e non sono Enitità, ovvero
                non hanno ID quindi non sono tabelle di un Database, ma campi. 
                Si mappano con 'OwnsOne' e se rispetti la convenzione basta fare solo 'entity.OwnsOne(course => course.CurrentPrice);'; infatti 
                dato che EFCore trova un OwnedType che nel nostro caso è di tipo 'Money'con proprietà 'Amount' e 'Currency'va a cercare 
                nel database colonne che si chiamano 'CurrentPrice_Amount' e 'CurrentPrice_Currency'.
                Se avessi dato nomi diversi alle colonne nel database per questi valori, invece della riga sotto il mapping sarebbe stato:

                entity.OwnsOne(course => course.CurrentPrice, builder => {
                    builder.Property(money => money.Currency).HasColumnName("NomeColonna_Currency");
                    builder.Property(money => money.Amount).HasColumnName("NomeColonna_Amount");
                });
                */

                // entity.OwnsOne(course => course.CurrentPrice);

                // avendo noi un campo di tipo 'enum', ovvero 'Currency', che nella tabella del database è convertito in tipo 'TEXT',
                // dobbiamo informare EFCore della conversione da fare quando scrive su database e viceversa.
                entity.OwnsOne(course => course.CurrentPrice, builder => {
                    builder.Property(money => money.Currency).HasConversion<string>();
                });

                entity.OwnsOne(course => course.FullPrice, builder => {
                    builder.Property(money => money.Currency).HasConversion<string>();
                });
                #endregion

                #region Mapping per le Relazioni (1 a Molti, ...)
                /*
                Il campo 'Lessons' nell'entità(Tabella) 'Courses' è un campo relazionale, da cui posso accedere
                a tutte le lezioni di un corso, dal corso stesso. Devo però mapparlo adeguatamente, come campo di Relazione.
                */
                entity.HasMany(course => course.Lessons)
                        .WithOne(lesson => lesson.Course)
                        .HasForeignKey(lesson => lesson.CourseId); // superfluo se il campo si chiama CourseId
                // L'entità Courses ha molte Lessons, a cui posso accedere tramite campo 'Lessons' della sua tabella;
                // a loro volta le lezione hanno solo un Course a cui fare riferimento, ed è il campo Course presente nella loro tabella(related_name di Django)
                // Specifico in aggiunta che la ForeignKey in Lessons è il campo CourseId appositamente creato, ma seguendo le convezioni è 
                // superfluo farlo in quanto se si chiama come l'entità a cui fa riferimento più il soffito Id (appunto CourseId) EFCore
                // cerca già automanticamente quel campo
                
                // Il mapping delle relazioni lo fai solo una volta, o dal punto di vista di Courses come abbiamo fatto noi o nell'entità
                // Lessons; in quel caso sarebbe stata:

                // entity.HasOne(course => course.Course)
                //         .WithMany(lesson => lesson.Lessons);
                #endregion

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
