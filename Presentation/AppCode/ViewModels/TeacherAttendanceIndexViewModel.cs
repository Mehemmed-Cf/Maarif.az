using Application.Modules.AttendanceModule;
using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Domain.Models.Stables;

namespace Presentation.AppCode.ViewModels
{
    public class TeacherAttendanceIndexViewModel
    {
        public WeekType CurrentWeekType { get; set; }
        public List<ScheduleDayDto> Days { get; set; } = new();
        public IReadOnlyList<AttendanceSessionListItemDto> Sessions { get; set; } = Array.Empty<AttendanceSessionListItemDto>();
    }
}
