using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class SubmissionFile : AuditableEntity
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
    }
}
