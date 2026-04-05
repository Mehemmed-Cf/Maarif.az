using Domain.Models.Stables;

namespace Application.Modules.LessonSchedulesModule
{
    public class LessonScheduleResponseDto
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int GroupId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomId { get; set; }
        public LessonType LessonType { get; set; }
        public WeekType WeekType { get; set; }
    }
}
