using System.Linq;
using Domain.Models.Stables;
using System.Collections.Generic;

namespace Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup
{
    public class LessonScheduleDto
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string TeacherFullName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomId { get; set; }
        public string RoomDisplay { get; set; }
        public LessonType LessonType { get; set; }
        public WeekType WeekType { get; set; }
    }

    public class ScheduleDayDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<LessonScheduleDto> Lessons { get; set; }
    }

    public class GetScheduleByGroupResponseDto
    {
        public WeekType CurrentWeekType { get; set; }
        public List<ScheduleDayDto> Days { get; set; }
    }
}
