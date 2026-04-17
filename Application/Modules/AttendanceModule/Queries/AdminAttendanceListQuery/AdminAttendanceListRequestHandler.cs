using Application.Repositories;
using MediatR;

namespace Application.Modules.AttendanceModule.Queries.AdminAttendanceListQuery
{
    public class AdminAttendanceListRequestHandler : IRequestHandler<AdminAttendanceListRequest, IReadOnlyList<AdminAttendanceListItemDto>>
    {
        private readonly IAttendanceRepository attendanceRepository;

        public AdminAttendanceListRequestHandler(IAttendanceRepository attendanceRepository)
        {
            this.attendanceRepository = attendanceRepository;
        }

        public async Task<IReadOnlyList<AdminAttendanceListItemDto>> Handle(AdminAttendanceListRequest request, CancellationToken cancellationToken)
        {
            await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);
            return await attendanceRepository.GetAdminAttendanceAsync(cancellationToken);
        }
    }
}
