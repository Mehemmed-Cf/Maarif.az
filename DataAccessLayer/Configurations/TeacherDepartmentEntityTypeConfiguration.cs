using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class TeacherDepartmentEntityTypeConfiguration : IEntityTypeConfiguration<TeacherDepartment>
    {
        public void Configure(EntityTypeBuilder<TeacherDepartment> builder)
        {
            builder.HasKey(td => new { td.TeacherId, td.DepartmentId });

            builder.Property(td => td.TeacherId).HasColumnType("int").IsRequired();
            builder.Property(td => td.DepartmentId).HasColumnType("int").IsRequired();

            builder.HasOne(td => td.Teacher)
            .WithMany(t => t.TeacherDepartments)
            .HasForeignKey(td => td.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(td => td.Department)
            .WithMany(d => d.TeacherDepartments)
            .HasForeignKey(td => td.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("TeacherDepartments", "dbo");
        }
    }
}