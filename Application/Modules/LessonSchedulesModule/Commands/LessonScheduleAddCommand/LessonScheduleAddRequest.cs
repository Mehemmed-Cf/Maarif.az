using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Commands.LessonScheduleAddCommand
{
    public class LessonScheduleAddRequest : IRequest<LessonScheduleResponseDto>
    {
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