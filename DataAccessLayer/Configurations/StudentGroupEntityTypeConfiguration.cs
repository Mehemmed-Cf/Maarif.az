using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> builder)
        {
            builder.ToTable("StudentGroups");

            // Composite PK — a student can only be in a specific group once.
            builder.HasKey(sg => new { sg.StudentId, sg.GroupId });

            builder.HasOne(sg => sg.Student)
                   .WithMany(s => s.StudentGroups)
                   .HasForeignKey(sg => sg.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sg => sg.Group)
                   .WithMany(g => g.StudentGroups)
                   .HasForeignKey(sg => sg.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sg => sg.GroupId);

            // StudentGroup inherits AuditableEntity — configure audit columns
            builder.Property(sg => sg.CreatedAt).IsRequired();
            builder.Property(sg => sg.CreatedBy).HasMaxLength(100);
            builder.Property(sg => sg.LastModifiedBy).HasMaxLength(100);
            builder.Property(sg => sg.DeletedBy).HasMaxLength(100);

            builder.HasQueryFilter(sg => sg.DeletedAt == null);
        }
    }
}