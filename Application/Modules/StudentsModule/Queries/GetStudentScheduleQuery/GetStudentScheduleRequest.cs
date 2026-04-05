using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.StudentsModule.Queries.GetStudentScheduleQuery
{
    public class GetStudentScheduleRequest : IRequest<StudentScheduleResponseDto>
    {
        public int UserId { get; set; }
    }

    public class StudentScheduleResponseDto
    {
        public WeekType CurrentWeekType { get; set; }
        public List<ScheduleDayDto> Days { get; set; } = new();
    }
}
