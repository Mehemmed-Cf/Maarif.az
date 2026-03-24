using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasQueryFilter(d => d.DeletedAt == null);

            builder.HasOne(d => d.Faculty)
                   .WithMany(f => f.Departments)
                   .HasForeignKey(d => d.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.FacultyId);
            builder.HasIndex(d => d.Name);
        }
    }
}