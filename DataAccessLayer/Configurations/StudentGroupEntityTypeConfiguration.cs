using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentGroupEntityTypeConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> builder)
        {
            builder.HasKey(sg => new { sg.StudentId, sg.GroupId });

            builder.Property(sg => sg.StudentId).HasColumnType("int").IsRequired();
            builder.Property(sg => sg.GroupId).HasColumnType("int").IsRequired();

            builder.HasOne(sg => sg.Student)
                .WithMany(s => s.StudentGroups)
                .HasForeignKey(sg => sg.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sg => sg.Group)
                .WithMany(g => g.StudentGroups)
                .HasForeignKey(sg => sg.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("StudentGroups", "dbo");
        }
    }
}