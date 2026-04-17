using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class Attendance : AuditableEntity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int LessonScheduleId { get; set; }
        public LessonSchedule LessonSchedule { get; set; }
        public DateTime SessionDate { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime MarkedAt { get; set; }
        public DateTime LockAt { get; set; }
        public bool IsLocked { get; set; }
        public int MarkedByTeacherId { get; set; }
        public Teacher MarkedByTeacher { get; set; }
        public ICollection<AttendanceAudit> AuditLogs { get; set; }
    }
}
