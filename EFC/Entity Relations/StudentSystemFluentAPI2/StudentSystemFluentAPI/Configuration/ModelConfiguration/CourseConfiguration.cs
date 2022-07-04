using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
namespace P01_StudentSystem.Configuration.ModelConfiguration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> modelBuilder)
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
        }
    }
}
