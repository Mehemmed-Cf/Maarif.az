using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubmissionFileEntityTypeConfiguration : IEntityTypeConfiguration<SubmissionFile>
    {
        public void Configure(EntityTypeBuilder<SubmissionFile> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SubmissionId).IsRequired();

            builder.Property(x => x.FileName)
                   .HasMaxLength(255)
                   .IsRequired();
                   
            builder.Property(x => x.FilePath)
                   .HasMaxLength(500)
                   .IsRequired();
                   
            builder.Property(x => x.FileSize)
                   .IsRequired();

            // Explicitly linking back to Submission if needed, usually handles itself but you can enable it
            // builder.HasOne<Submission>().WithMany(x => x.SubmissionFiles).HasForeignKey(x => x.SubmissionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}