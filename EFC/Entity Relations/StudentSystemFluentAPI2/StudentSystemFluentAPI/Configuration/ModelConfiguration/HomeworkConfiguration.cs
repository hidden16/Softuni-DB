using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
namespace P01_StudentSystem.Configuration.ModelConfiguration
{
    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
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
                .HasForeignKey(x => x.StudentId);

            builder.HasOne(x=>x.Course)
                .WithMany(x=>x.HomeworkSubmissions)
                .HasForeignKey(x => x.CourseId);
        }
    }
}
