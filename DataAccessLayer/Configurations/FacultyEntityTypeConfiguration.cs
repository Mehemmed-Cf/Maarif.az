using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class FacultyEntityTypeConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.Property(f => f.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(f => f.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();

            builder.HasMany(f => f.Departments)
                .WithOne(d => d.Faculty)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Faculties", "dbo");
        }
    }
}