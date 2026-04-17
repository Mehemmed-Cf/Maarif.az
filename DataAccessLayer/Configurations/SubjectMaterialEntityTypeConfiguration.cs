using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubjectMaterialConfiguration : IEntityTypeConfiguration<SubjectMaterial>
    {
        public void Configure(EntityTypeBuilder<SubjectMaterial> builder)
        {
            builder.ToTable("SubjectMaterials");
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Title).IsRequired().HasMaxLength(200);
            builder.Property(sm => sm.Description).HasMaxLength(1000);
            builder.Property(sm => sm.FileUrl).IsRequired().HasMaxLength(500);
            builder.Property(sm => sm.MaterialType).HasMaxLength(50);

            builder.HasQueryFilter(sm => sm.DeletedAt == null);

            builder.HasOne(sm => sm.Subject)
                   .WithMany(s => s.Materials)
                   .HasForeignKey(sm => sm.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade); // Deleting a subject deletes its materials
        }
    }
}
