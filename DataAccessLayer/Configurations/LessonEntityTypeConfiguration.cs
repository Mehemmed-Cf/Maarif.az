using DataAccessLayer.Extensions;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(l => l.Id);

            builder.HasQueryFilter(l => l.DeletedAt == null);

            builder.HasOne(l => l.Teacher)
                   .WithMany(t => t.Lessons)
                   .HasForeignKey(l => l.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Subject)
                   .WithMany(s => s.Lessons)
                   .HasForeignKey(l => l.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            // A teacher should not teach the same subject twice as separate lesson records
            builder.HasIndex(l => new { l.TeacherId, l.SubjectId }).IsUniqueWhenNotDeleted();
            builder.HasIndex(l => l.SubjectId);
        }
    }
}