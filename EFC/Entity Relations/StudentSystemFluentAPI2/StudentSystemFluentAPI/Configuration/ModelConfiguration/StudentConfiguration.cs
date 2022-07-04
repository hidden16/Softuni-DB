using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
namespace P01_StudentSystem.Configuration.ModelConfiguration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
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
        }
    }
}
