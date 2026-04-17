using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubjectLiteratureConfiguration : IEntityTypeConfiguration<SubjectLiterature>
    {
        public void Configure(EntityTypeBuilder<SubjectLiterature> builder)
        {
            builder.ToTable("SubjectLiteratures");
            builder.HasKey(sl => sl.Id);

            builder.Property(sl => sl.Type).IsRequired().HasMaxLength(50);
            builder.Property(sl => sl.Author).IsRequired().HasMaxLength(200);
            builder.Property(sl => sl.BookName).IsRequired().HasMaxLength(300);
            builder.Property(sl => sl.Publisher).HasMaxLength(200);
            builder.Property(sl => sl.PublicationYear);

            builder.HasQueryFilter(sl => sl.DeletedAt == null);

            builder.HasOne(sl => sl.Subject)
                   .WithMany(s => s.Literatures)
                   .HasForeignKey(sl => sl.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
