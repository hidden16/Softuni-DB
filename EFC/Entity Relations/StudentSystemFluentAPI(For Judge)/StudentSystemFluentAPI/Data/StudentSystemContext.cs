using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        private const string connectionString = @"Server=IKSAN\SQLEXPRESS;Database=StudentSystem;Integrated Security=true;";
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(builder =>
            {
                builder.HasKey(x => x.StudentId);

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(x => x.PhoneNumber)
                    .HasColumnType("CHAR(10)");

                builder.Property(x => x.RegisteredOn)
                    .IsRequired();

                builder.Property(x => x.Birthday);
            });

            modelBuilder.Entity<Course>(modelBuilder =>
            {
                modelBuilder.HasKey(x => x.CourseId);

                modelBuilder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                modelBuilder.Property(x => x.Description);

                modelBuilder.Property(x => x.StartDate)
                    .IsRequired();

                modelBuilder.Property(x => x.EndDate)
                    .IsRequired();

                modelBuilder.Property(x => x.Price);
            });

            modelBuilder.Entity<Resource>(builder =>
            {
                builder.HasKey(x => x.ResourceId);

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(x => x.Url)
                    .IsRequired()
                    .HasColumnType("VARCHAR(MAX)");

                builder.Property(x => x.ResourceType)
                    .IsRequired();

                builder.Property(x => x.CourseId)
                    .IsRequired();

                builder.HasOne(x => x.Course)
                .WithMany(x => x.Resources)
                .HasForeignKey(x=>x.CourseId);
            });

            modelBuilder.Entity<Homework>(builder =>
            {
                builder.HasKey(x => x.HomeworkId);

                builder.Property(x => x.Content)
                    .IsRequired()
                    .HasColumnType("VARCHAR(MAX)");

                builder.Property(x => x.ContentType)
                    .IsRequired();

                builder.Property(x => x.SubmissionTime)
                    .IsRequired();

                builder.Property(x => x.StudentId)
                    .IsRequired();

                builder.Property(x => x.CourseId)
                    .IsRequired();

                builder.HasOne(x => x.Student)
                .WithMany(x => x.HomeworkSubmissions)
                .HasForeignKey(x=>x.StudentId);

                builder.HasOne(x => x.Course)
                .WithMany(x => x.HomeworkSubmissions)
                .HasForeignKey(x => x.HomeworkId);
            });

            modelBuilder.Entity<StudentCourse>(builder =>
            {
                builder.HasKey(x => new { x.CourseId, x.StudentId });
                builder.Property(x => x.StudentId);
                builder.Property(x => x.CourseId);

                builder.HasOne(x => x.Course)
                .WithMany(x => x.StudentsEnrolled)
                .HasForeignKey(x => x.CourseId);

                builder.HasOne(x => x.Student)
                .WithMany(x=>x.CourseEnrollments)
                .HasForeignKey(x=>x.StudentId);
            });
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
