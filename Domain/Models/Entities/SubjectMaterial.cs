using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class SubjectMaterial : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FileUrl { get; set; } = string.Empty; // Path to PDF, Video, etc.
        public string MaterialType { get; set; } = string.Empty; // e.g., "Syllabus", "LectureNote", "Assignment"

        // --- Relationships ---
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }
}
