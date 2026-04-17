using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class SubjectTopicConfiguration : IEntityTypeConfiguration<SubjectTopic>
    {
        public void Configure(EntityTypeBuilder<SubjectTopic> builder)
        {
            builder.ToTable("SubjectTopics");
            builder.HasKey(st => st.Id);

            builder.Property(st => st.WeekNumber).IsRequired();
            builder.Property(st => st.TopicName).IsRequired().HasMaxLength(200);
            builder.Property(st => st.TeachingMethods).HasMaxLength(1000);
            builder.Property(st => st.Materials).HasMaxLength(1000);
            builder.Property(st => st.Equipment).HasMaxLength(1000);

            builder.HasQueryFilter(st => st.DeletedAt == null);

            builder.HasOne(st => st.Subject)
                   .WithMany(s => s.Topics)
                   .HasForeignKey(st => st.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}