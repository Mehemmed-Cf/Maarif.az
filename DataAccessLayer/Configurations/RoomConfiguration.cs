using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Number).IsRequired();
            builder.Ignore(r => r.Floor);
            
            builder.HasOne(r => r.Building)
                   .WithMany(b => b.Rooms)
                   .HasForeignKey(r => r.BuildingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(r => r.DeletedAt == null);

            builder.ConfigureAuditable();
        }
    }
}
