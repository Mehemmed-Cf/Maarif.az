using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class AssignmentEntityTypeConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(4000).IsRequired(false);
            
            builder.Property(x => x.LessonId).IsRequired();
            
            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasConversion<int>(); // Storing Enum as int
                   
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();
            
            builder.Property(x => x.MaxGrade).IsRequired();
            builder.Property(x => x.AllowLateSubmission).IsRequired().HasDefaultValue(false);

            // Add navigation property mappings if you have collection properties defined on the Assignment class
            // builder.HasMany(x => x.AssignmentFiles).WithOne().HasForeignKey(x => x.AssignmentId).OnDelete(DeleteBehavior.Cascade);
            // builder.HasMany(x => x.Submissions).WithOne().HasForeignKey(x => x.AssignmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}