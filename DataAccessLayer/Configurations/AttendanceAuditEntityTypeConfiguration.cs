using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class AttendanceAuditEntityTypeConfiguration : IEntityTypeConfiguration<AttendanceAudit>
    {
        public void Configure(EntityTypeBuilder<AttendanceAudit> builder)
        {
            builder.ToTable("AttendanceAudits");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ChangedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(a => a.Note)
                .HasMaxLength(500);

            builder.Property(a => a.OldStatus)
                .IsRequired();

            builder.Property(a => a.NewStatus)
                .IsRequired();

            builder.Property(a => a.WasLockedBeforeChange)
                .IsRequired();

            builder.Property(a => a.WasAdminOverride)
                .IsRequired();

            builder.HasQueryFilter(a => a.DeletedAt == null);

            builder.ConfigureAuditable();
        }
    }
}
