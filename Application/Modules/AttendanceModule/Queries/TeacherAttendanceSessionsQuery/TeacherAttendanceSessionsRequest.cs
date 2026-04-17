using MediatR;

namespace Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionsQuery
{
    public class TeacherAttendanceSessionsRequest : IRequest<IReadOnlyList<AttendanceSessionListItemDto>>
    {
        public int UserId { get; set; }
    }
}
