using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class TeacherDepartmentConfiguration : IEntityTypeConfiguration<TeacherDepartment>
    {
        public void Configure(EntityTypeBuilder<TeacherDepartment> builder)
        {
            builder.ToTable("TeacherDepartments");

            // Composite PK — prevents a teacher from being assigned to the same
            // department twice without needing a separate surrogate Id column.
            builder.HasKey(td => new { td.TeacherId, td.DepartmentId });

            builder.HasOne(td => td.Teacher)
                   .WithMany(t => t.TeacherDepartments)
                   .HasForeignKey(td => td.TeacherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(td => td.Department)
                   .WithMany(d => d.TeacherDepartments)
                   .HasForeignKey(td => td.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(td => td.DepartmentId);
        }
    }
}