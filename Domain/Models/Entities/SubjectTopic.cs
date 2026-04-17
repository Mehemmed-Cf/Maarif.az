using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class SubjectTopic : AuditableEntity
    {
        public int Id { get; set; }
        public int WeekNumber { get; set; }      // Həftə
        public string TopicName { get; set; }    // Mövzu
        public string? TeachingMethods { get; set; } // Tədris metodları
        public string? Materials { get; set; }    // Didaktik materiallar
        public string? Equipment { get; set; }    // Avadanlıq

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
