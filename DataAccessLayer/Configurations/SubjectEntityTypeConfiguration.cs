using DataAccessLayer.Extensions;
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

            builder.HasQueryFilter(s => s.DeletedAt == null);

            // Header Info
            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.Term)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.GroupName)
                   .IsRequired()
                   .HasMaxLength(100);

            // Teachers
            builder.Property(s => s.LectureTeacher)
                   .HasMaxLength(150);

            builder.Property(s => s.SeminarTeacher)
                   .HasMaxLength(150);

            builder.Property(s => s.LabTeacher)
                   .HasMaxLength(150);

            // Statistics
            builder.Property(s => s.StudentCount)
                   .IsRequired();

            builder.Property(s => s.Credits)
                   .IsRequired();

            builder.Property(s => s.TotalHours)
                   .IsRequired();

            builder.Property(s => s.WeekCount)
                   .IsRequired();

            // Descriptions
            builder.Property(s => s.Purpose)
                   .HasMaxLength(2000);

            builder.Property(s => s.TeacherMethods)
                   .HasMaxLength(2000);

            builder.Property(s => s.SyllabusUrl)
                   .HasMaxLength(500);

            // Grading
            builder.Property(s => s.FreeWorkScore)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(s => s.SeminarScore)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(s => s.LabScore)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(s => s.AttendanceScore)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(s => s.ExamScore)
                   .IsRequired()
                   .HasDefaultValue(50);

            // Relationships
            builder.HasOne(s => s.Department)
                   .WithMany(d => d.Subjects)
                   .HasForeignKey(s => s.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.DepartmentId);

            // Unique subject name inside one department
            builder.HasIndex(s => new { s.DepartmentId, s.Name }).IsUniqueWhenNotDeleted();

            // Optional: make duplicate groups harder to create inside same department/term
            builder.HasIndex(s => new { s.DepartmentId, s.Term, s.GroupName }).IsUniqueWhenNotDeleted();

            // Optional check constraints for grading
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_Subject_Scores_NonNegative",
                    "[FreeWorkScore] >= 0 AND [SeminarScore] >= 0 AND [LabScore] >= 0 AND [AttendanceScore] >= 0 AND [ExamScore] >= 0");

                tb.HasCheckConstraint("CK_Subject_Stats_NonNegative",
                    "[StudentCount] >= 0 AND [Credits] >= 0 AND [TotalHours] >= 0 AND [WeekCount] >= 0");
            });
        }
    }
}



//using Domain.Models.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace DataAccessLayer.Configurations
//{
//    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
//    {
//        public void Configure(EntityTypeBuilder<Subject> builder)
//        {
//            builder.ToTable("Subjects");

//            builder.HasKey(s => s.Id);

//            builder.Property(s => s.Name)
//                   .IsRequired()
//                   .HasMaxLength(200);

//            builder.HasQueryFilter(s => s.DeletedAt == null);

//            builder.HasOne(s => s.Department)
//                   .WithMany(d => d.Subjects)
//                   .HasForeignKey(s => s.DepartmentId)
//                   .OnDelete(DeleteBehavior.Restrict);

//            builder.HasIndex(s => s.DepartmentId);
//            // Subject names unique within a department (e.g. two depts can both have "Math 101")
//            builder.HasIndex(s => new { s.DepartmentId, s.Name }).IsUnique();
//        }
//    }
//}

