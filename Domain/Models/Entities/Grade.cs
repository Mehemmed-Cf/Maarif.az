using Domain.Models.Concrates;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Grade : AuditableEntity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }

        // Logic Settings
        public bool HasLaboratory { get; set; }

        // Input Scores
        public int? ManualPracticalScore { get; set; }
        public int? FreelanceWork { get; set; }
        public int? MidtermScore { get; set; }
        public int? LaboratoryScore { get; set; }
        public int? ExamScore { get; set; }

        // ── Computed (Not Stored in DB) ──

        // This would likely come from a service or a joined table in a real app
        public int AttendanceScore { get; set; }

        public int? SemesterTotal =>
            AttendanceScore + ManualPracticalScore + (MidtermScore ?? 0) + (LaboratoryScore ?? 0);

        [NotMapped]
        public int? GrandTotal =>
            SemesterTotal + (ExamScore ?? 0);
    }
}
