using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models.Entities.Membership;

namespace DataAccessLayer.Configurations.Membership
{
    class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(m => m.UserName).HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(m => m.NormalizedUserName).HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(m => m.PasswordHash).HasColumnType("varchar").HasMaxLength(400).IsRequired();
            builder.Property(m => m.SecurityStamp).HasColumnType("varchar").HasMaxLength(400).IsRequired();
            builder.Property(m => m.ConcurrencyStamp).HasColumnType("varchar").HasMaxLength(400).IsRequired();
            builder.Property(m => m.LockoutEnd).HasColumnType("datetimeoffset");
            builder.Property(m => m.LockoutEnabled).HasColumnType("bit").IsRequired();
            builder.Property(m => m.AccessFailedCount).HasColumnType("int").IsRequired();

            builder.HasKey(m => m.Id);
            builder.ToTable("Users", "Membership");
        }
    }
}
