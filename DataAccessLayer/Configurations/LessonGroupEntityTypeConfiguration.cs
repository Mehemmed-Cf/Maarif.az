using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class LessonGroupConfiguration : IEntityTypeConfiguration<LessonGroup>
    {
        public void Configure(EntityTypeBuilder<LessonGroup> builder)
        {
            builder.ToTable("LessonGroups");

            // Composite PK — a lesson can only be assigned to a specific group once.
            builder.HasKey(lg => new { lg.LessonId, lg.GroupId });

            builder.HasOne(lg => lg.Lesson)
                   .WithMany(l => l.LessonGroups)
                   .HasForeignKey(lg => lg.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lg => lg.Group)
                   .WithMany(g => g.LessonGroups)
                   .HasForeignKey(lg => lg.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(lg => lg.GroupId);
        }
    }
}