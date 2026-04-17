using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class PracticalRecord : AuditableEntity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int Score { get; set; }
    }
}
