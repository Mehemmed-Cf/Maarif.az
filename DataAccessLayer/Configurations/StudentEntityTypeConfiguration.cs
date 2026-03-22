using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(s => s.Id).HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(s => s.FullName).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(s => s.FatherName).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(s => s.Gender).HasColumnType("tinyint").IsRequired();
            builder.Property(s => s.MobileNumber).HasColumnType("nvarchar").HasMaxLength(15).IsRequired();
            builder.Property(s => s.BirthDate).HasColumnType("datetime2").IsRequired();
            builder.Property(s => s.EducationType).HasColumnType("tinyint").IsRequired();
            builder.Property(s => s.Status).HasColumnType("tinyint").IsRequired();
            builder.Property(s => s.Year).HasColumnType("tinyint").IsRequired();
            builder.Property(s => s.UserId).HasColumnType("int").IsRequired();
            builder.Property(s => s.Grade).HasColumnType("tinyint").IsRequired();
            builder.Property(s => s.DepartmentId).HasColumnType("int").IsRequired();
            builder.Property(s => s.FacultyId).HasColumnType("int").IsRequired();

            builder.HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Faculty)
                .WithMany()
                .HasForeignKey(s => s.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.StudentGroups)
                .WithOne(sg => sg.Student)
                .HasForeignKey(sg => sg.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Students", "dbo");
        }
    }
}