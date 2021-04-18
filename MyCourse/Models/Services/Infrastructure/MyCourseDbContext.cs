using Microsoft.EntityFrameworkCore;
using MyCourse.Models.Entities;

namespace MyCourse.Models.Services.Infrastructure
{
    public partial class MyCourseDbContext : DbContext
    {
        public MyCourseDbContext(DbContextOptions<MyCourseDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");
            modelBuilder.Entity<Course>(entity =>
            {
                entity.OwnsOne(course => course.CurrentPrice, builder => {
                    builder.Property(money => money.Currency).HasConversion<string>();
                });

                entity.OwnsOne(course => course.FullPrice, builder => {
                    builder.Property(money => money.Currency).HasConversion<string>();
                });

                entity.HasMany(course => course.Lessons)
                        .WithOne(lesson => lesson.Course);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                
            });
        }
    }
}
