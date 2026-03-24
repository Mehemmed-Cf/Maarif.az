using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class StudentGroup : AuditableEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
