using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionsQuery
{
    public class TeacherAttendanceSessionsRequestHandler : IRequestHandler<TeacherAttendanceSessionsRequest, IReadOnlyList<AttendanceSessionListItemDto>>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IAttendanceRepository attendanceRepository;

        public TeacherAttendanceSessionsRequestHandler(
            ITeacherRepository teacherRepository,
            IAttendanceRepository attendanceRepository)
        {
            this.teacherRepository = teacherRepository;
            this.attendanceRepository = attendanceRepository;
        }

        public async Task<IReadOnlyList<AttendanceSessionListItemDto>> Handle(TeacherAttendanceSessionsRequest request, CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("Teacher account was not found.");

            await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);
            return await attendanceRepository.GetTeacherSessionsAsync(teacher.Id, cancellationToken);
        }
    }
}
