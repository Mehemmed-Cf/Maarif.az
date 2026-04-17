using MediatR;

namespace Application.Modules.AttendanceModule.Queries.StudentAttendanceQuery
{
    public class StudentAttendanceRequest : IRequest<IReadOnlyList<StudentAttendanceListItemDto>>
    {
        public int UserId { get; set; }
    }
}
