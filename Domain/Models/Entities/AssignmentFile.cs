using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class AssignmentFile : AuditableEntity
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
    }
}
