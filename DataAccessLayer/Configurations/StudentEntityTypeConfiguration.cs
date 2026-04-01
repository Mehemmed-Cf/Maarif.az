using Domain.Models.Entities;
using Domain.Models.Stables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.FullName)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(s => s.FatherName)
                   .HasMaxLength(200);

            builder.Property(s => s.FinCode)
            .IsRequired()
            .HasMaxLength(7);

            builder.HasIndex(s => s.FinCode)
            .IsUnique();  // one student per FIN, enforced at DB level too

            builder.Property(s => s.StudentNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.MobileNumber)
                   .HasMaxLength(20)
                    .IsRequired(false); //

            builder.Property(s => s.Gender)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(s => s.EducationType)
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(s => s.Status)
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(s => s.Grade)
                   .HasConversion<string>()
                   .HasMaxLength(10);

            builder.HasQueryFilter(s => s.DeletedAt == null);

            // Department is the single source of truth for faculty.
            // No FacultyId column here — derive via Department.Faculty when needed.
            builder.HasOne(s => s.Department)
                   .WithMany(d => d.Students)
                   .HasForeignKey(s => s.DepartmentId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.StudentNumber).IsUnique();
            builder.HasIndex(s => s.DepartmentId);
            builder.HasIndex(s => s.UserId);
        }
    }
}