using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subjects");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasQueryFilter(s => s.DeletedAt == null);

            builder.HasOne(s => s.Department)
                   .WithMany(d => d.Subjects)
                   .HasForeignKey(s => s.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.DepartmentId);
            // Subject names unique within a department (e.g. two depts can both have "Math 101")
            builder.HasIndex(s => new { s.DepartmentId, s.Name }).IsUnique();
        }
    }
}