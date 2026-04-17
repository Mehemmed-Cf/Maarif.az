using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class AttendanceAudit : AuditableEntity
    {
        public int Id { get; set; }
        public int AttendanceId { get; set; }
        public Attendance Attendance { get; set; }
        public AttendanceStatus OldStatus { get; set; }
        public AttendanceStatus NewStatus { get; set; }
        public bool WasLockedBeforeChange { get; set; }
        public bool WasAdminOverride { get; set; }
        public DateTime ChangedAt { get; set; }
        public int ChangedByUserId { get; set; }
        public string? Note { get; set; }
    }
}
