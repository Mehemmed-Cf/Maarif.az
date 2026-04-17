using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class Assignment : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LessonId { get; set; }
        public AssignmentType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int MaxGrade{ get; set; }
        public bool AllowLateSubmission { get; set; }
    }
}
