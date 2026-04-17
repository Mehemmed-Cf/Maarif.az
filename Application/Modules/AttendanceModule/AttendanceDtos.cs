using Domain.Models.Stables;

namespace Application.Modules.AttendanceModule
{
    public class AttendanceListItemDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
        public bool HasRecord { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime MarkedAt { get; set; }
        public DateTime LockAt { get; set; }
        public bool IsLocked { get; set; }
    }

    public class AttendanceSessionListItemDto
    {
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public string RoomDisplay { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AttendanceCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public bool HasLockedRecords { get; set; }
    }

    public class AttendanceSessionDetailsDto
    {
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public string TeacherFullName { get; set; }
        public string RoomDisplay { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsSessionLocked { get; set; }
        public DateTime? SessionMarkedAt { get; set; }
        public DateTime? SessionLockAt { get; set; }
        public bool IsTeacherOwner { get; set; }
        public List<AttendanceListItemDto> Students { get; set; } = new();
    }

    public class StudentAttendanceListItemDto
    {
        public int AttendanceId { get; set; }
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SubjectName { get; set; }
        public string TeacherFullName { get; set; }
        public string GroupName { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime MarkedAt { get; set; }
        public DateTime LockAt { get; set; }
        public bool IsLocked { get; set; }
    }

    public class AdminAttendanceListItemDto
    {
        public int AttendanceId { get; set; }
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
        public string TeacherFullName { get; set; }
        public AttendanceStatus Status { get; set; }
        public bool IsLocked { get; set; }
        public DateTime MarkedAt { get; set; }
        public DateTime LockAt { get; set; }
    }

    public class AttendanceAuditLogDto
    {
        public int Id { get; set; }
        public AttendanceStatus OldStatus { get; set; }
        public AttendanceStatus NewStatus { get; set; }
        public bool WasLockedBeforeChange { get; set; }
        public bool WasAdminOverride { get; set; }
        public DateTime ChangedAt { get; set; }
        public int ChangedByUserId { get; set; }
        public string? Note { get; set; }
    }

    public class AttendanceDetailsDto
    {
        public int Id { get; set; }
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
        public string TeacherFullName { get; set; }
        public AttendanceStatus Status { get; set; }
        public bool IsLocked { get; set; }
        public DateTime MarkedAt { get; set; }
        public DateTime LockAt { get; set; }
        public List<AttendanceAuditLogDto> AuditLogs { get; set; } = new();
    }
}
