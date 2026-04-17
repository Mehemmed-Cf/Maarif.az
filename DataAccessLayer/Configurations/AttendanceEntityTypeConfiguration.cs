using DataAccessLayer.Extensions;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataAccessLayer.Configurations
{
    public class AttendanceEntityTypeConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.SessionDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(a => a.MarkedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(a => a.LockAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(a => a.IsLocked)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasIndex(a => new { a.LessonScheduleId, a.StudentId, a.SessionDate })
                .IsUniqueWhenNotDeleted();

            builder.HasIndex(a => new { a.LessonScheduleId, a.SessionDate });

            builder.HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.LessonSchedule)
                .WithMany(ls => ls.Attendances)
                .HasForeignKey(a => a.LessonScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.MarkedByTeacher)
                .WithMany(t => t.MarkedAttendances)
                .HasForeignKey(a => a.MarkedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.AuditLogs)
                .WithOne(al => al.Attendance)
                .HasForeignKey(al => al.AttendanceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(a => a.DeletedAt == null);

            builder.ConfigureAuditable();
        }
    }
}
