using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class Submission : AuditableEntity
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string Note { get; set; }
        public string LinkUrl { get; set; }
        public int? Grade { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? GradedAt { get; set; }
        public int? GradedByUserId { get; set; }
        public Status Status { get; set; }
    }
}
