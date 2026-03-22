using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class TeacherEntityTypeConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(t => t.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(t => t.FullName).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(t => t.MobileNumber).HasColumnType("nvarchar").HasMaxLength(15);
            builder.Property(t => t.Email).HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(t => t.BirthDate).HasColumnType("datetime2").IsRequired();
            builder.Property(t => t.GroupCount).HasColumnType("int").IsRequired();
            builder.Property(t => t.ActiveLessons).HasColumnType("int").IsRequired();
            builder.Property(t => t.Experience).HasColumnType("float").IsRequired();
            builder.Property(t => t.UserId).HasColumnType("int").IsRequired();

            builder.HasMany(t => t.TeacherDepartments)
                .WithOne(td => td.Teacher)
                .HasForeignKey(td => td.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Lessons)
                .WithOne(l => l.Teacher)
                .HasForeignKey(l => l.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Teachers", "dbo");
        }
    }
}