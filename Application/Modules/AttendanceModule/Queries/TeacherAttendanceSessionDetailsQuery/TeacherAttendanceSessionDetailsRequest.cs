using MediatR;

namespace Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionDetailsQuery
{
    public class TeacherAttendanceSessionDetailsRequest : IRequest<AttendanceSessionDetailsDto>
    {
        public int UserId { get; set; }
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
    }
}
