using Application.Modules.AttendanceModule;

namespace Presentation.AppCode.ViewModels
{
    public class TeacherAttendanceSessionViewModel
    {
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string TeacherFullName { get; set; } = string.Empty;
        public string RoomDisplay { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsSessionLocked { get; set; }
        public DateTime? SessionMarkedAt { get; set; }
        public DateTime? SessionLockAt { get; set; }
        public List<AttendanceListItemDto> Students { get; set; } = new();
    }
}
