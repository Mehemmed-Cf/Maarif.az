using DataAccessLayer.Extensions;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(g => g.Year)
                   .IsRequired();

            // StudentCount is [NotMapped]  EF ignores it automatically.

            builder.HasQueryFilter(g => g.DeletedAt == null);

            builder.HasOne(g => g.Department)
                   .WithMany(d => d.Groups)
                   .HasForeignKey(g => g.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(g => g.DepartmentId);
            // A group name like "IT-101" should be unique within a department
            builder.HasIndex(g => new { g.DepartmentId, g.Name }).IsUniqueWhenNotDeleted();
        }
    }
}