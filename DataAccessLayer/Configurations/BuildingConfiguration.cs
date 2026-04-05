using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.ToTable("Buildings");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            
            builder.HasMany(b => b.Rooms)
                   .WithOne(r => r.Building)
                   .HasForeignKey(r => r.BuildingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
