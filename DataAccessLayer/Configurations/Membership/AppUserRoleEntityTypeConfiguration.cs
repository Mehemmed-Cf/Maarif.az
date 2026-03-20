using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models.Entities.Membership;

namespace DataAccessLayer.Configurations.Membership
{
    class AppUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.Property(m => m.UserId).HasColumnType("int");
            builder.Property(m => m.RoleId).HasColumnType("int");

            builder.HasKey(m => new { m.UserId, m.RoleId });
            builder.ToTable("UserRoles", "Membership");

            builder.HasOne<AppUser>()
                .WithMany()
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<AppRole>()
                .WithMany()
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(m => m.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
