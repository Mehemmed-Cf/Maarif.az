using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class SubjectLiterature : AuditableEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } // "Əsas" (Main) or "Köməkçi" (Auxiliary)
        public string Author { get; set; }
        public string BookName { get; set; }
        public string? Publisher { get; set; }
        public int? PublicationYear { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
