using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.TeachersModule.Queries.GetTeacherScheduleQuery
{
    public class GetTeacherScheduleRequest : IRequest<TeacherScheduleResponseDto>
    {
        public int UserId { get; set; }
    }

    public class TeacherScheduleResponseDto
    {
        public WeekType CurrentWeekType { get; set; }
        public List<ScheduleDayDto> Days { get; set; } = new();
    }
}
