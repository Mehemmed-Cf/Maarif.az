using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(g => g.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(g => g.Name).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Property(g => g.Year).HasColumnType("tinyint").IsRequired();
            builder.Property(g => g.DepartmentId).HasColumnType("int").IsRequired();

            builder.HasOne(g => g.Department)
                .WithMany(d => d.Groups)
                .HasForeignKey(g => g.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.StudentGroups)
                .WithOne(sg => sg.Group)
                .HasForeignKey(sg => sg.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Lessons)
                .WithOne(l => l.Group)
                .HasForeignKey(l => l.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Groups", "dbo");
        }
    }
}