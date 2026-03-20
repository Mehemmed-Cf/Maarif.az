using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models.Entities.Membership;

namespace DataAccessLayer.Configurations.Membership
{
    class AppUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<AppUserToken>
    {
        public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            builder.Property(m => m.UserId).HasColumnType("int");
            builder.Property(m => m.LoginProvider).HasColumnType("nvarchar").HasMaxLength(450);
            builder.Property(m => m.Name).HasColumnType("nvarchar").HasMaxLength(450);
            builder.Property(m => m.Value).HasColumnType("nvarchar").HasMaxLength(450);
            builder.Property(m => m.IsActive).HasColumnType("bit").HasDefaultValue(true);
            builder.Property(m => m.Expired).HasColumnType("datetime");

            builder.HasKey(m => new { m.UserId, m.LoginProvider, m.Name, m.Value });
            builder.ToTable("UserTokens", "Membership");

            builder.HasOne<AppUser>()
                .WithMany()
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
