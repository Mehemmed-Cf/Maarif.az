using MediatR;

namespace Application.Modules.AttendanceModule.Queries.AdminAttendanceListQuery
{
    public class AdminAttendanceListRequest : IRequest<IReadOnlyList<AdminAttendanceListItemDto>>
    {
    }
}
