using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.AttendanceModule.Queries.StudentAttendanceQuery
{
    public class StudentAttendanceRequestHandler : IRequestHandler<StudentAttendanceRequest, IReadOnlyList<StudentAttendanceListItemDto>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IAttendanceRepository attendanceRepository;

        public StudentAttendanceRequestHandler(
            IStudentRepository studentRepository,
            IAttendanceRepository attendanceRepository)
        {
            this.studentRepository = studentRepository;
            this.attendanceRepository = attendanceRepository;
        }

        public async Task<IReadOnlyList<StudentAttendanceListItemDto>> Handle(StudentAttendanceRequest request, CancellationToken cancellationToken)
        {
            var student = await studentRepository.GetByUserIdWithDetailsAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("Student account was not found.");

            await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);
            return await attendanceRepository.GetStudentAttendanceAsync(student.Id, cancellationToken);
        }
    }
}
