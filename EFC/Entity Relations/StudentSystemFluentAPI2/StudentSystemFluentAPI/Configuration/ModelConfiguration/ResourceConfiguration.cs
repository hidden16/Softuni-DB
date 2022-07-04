using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
namespace P01_StudentSystem.Configuration.ModelConfiguration
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
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
                .HasForeignKey(x => x.CourseId);
        }
    }
}
