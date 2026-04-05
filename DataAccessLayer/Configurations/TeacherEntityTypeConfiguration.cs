using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.FullName)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(t => t.FinCode)
                   .HasMaxLength(7);

            builder.Property(t => t.DocumentSerialNumber)
                .HasMaxLength(40);

            builder.Property(t => t.TeacherNumber)
                   .HasMaxLength(32);

            builder.Property(t => t.MobileNumber)
                   .HasMaxLength(20);

            builder.Property(t => t.Email)
                   .HasMaxLength(256);

            builder.Property(t => t.Experience)
                   .HasColumnType("decimal(5,2)");

            // GroupCount and ActiveLessons are [NotMapped]  EF will ignore them.
            // They are computed in memory from navigation collections, or via
            // dedicated Dapper aggregation queries in the repository for list views.

            builder.HasQueryFilter(t => t.DeletedAt == null);

            builder.HasIndex(t => t.Email).IsUnique();
            builder.HasIndex(t => t.UserId);
            builder.HasIndex(t => t.FinCode).IsUnique();
            builder.HasIndex(t => t.TeacherNumber).IsUnique();

            builder.HasIndex(t => t.DocumentSerialNumber)
                .IsUnique()
                .HasFilter("[DocumentSerialNumber] IS NOT NULL");
        }
    }
}