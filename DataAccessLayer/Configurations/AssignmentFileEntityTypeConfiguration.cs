using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class AssignmentFileEntityTypeConfiguration : IEntityTypeConfiguration<AssignmentFile>
    {
        public void Configure(EntityTypeBuilder<AssignmentFile> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.AssignmentId).IsRequired();

            builder.Property(x => x.FileName)
                   .HasMaxLength(255)
                   .IsRequired();
                   
            builder.Property(x => x.FilePath)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.FileSize)
                   .IsRequired();
                   
            // If you have a navigation property for Assignment -> AssignmentFiles, EF might resolve this automatically, but you can explicitly define it here.
            // builder.HasOne<Assignment>().WithMany(x => x.AssignmentFiles).HasForeignKey(x => x.AssignmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}