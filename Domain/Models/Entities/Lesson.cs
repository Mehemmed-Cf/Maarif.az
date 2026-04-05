using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Lesson : AuditableEntity
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<LessonGroup> LessonGroups { get; set; }
        public ICollection<LessonSchedule> LessonSchedules { get; set; }
    }
}
