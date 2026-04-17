using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.AttendanceModule.Queries.AttendanceGetByIdQuery
{
    public class AttendanceGetByIdRequestHandler : IRequestHandler<AttendanceGetByIdRequest, AttendanceDetailsDto>
    {
        private readonly IAttendanceRepository attendanceRepository;

        public AttendanceGetByIdRequestHandler(IAttendanceRepository attendanceRepository)
        {
            this.attendanceRepository = attendanceRepository;
        }

        public async Task<AttendanceDetailsDto> Handle(AttendanceGetByIdRequest request, CancellationToken cancellationToken)
        {
            await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);

            var attendance = await attendanceRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Attendance record was not found.");

            return new AttendanceDetailsDto
            {
                Id = attendance.Id,
                LessonScheduleId = attendance.LessonScheduleId,
                SessionDate = attendance.SessionDate,
                SubjectName = attendance.LessonSchedule.Lesson.Subject.Name,
                GroupName = attendance.LessonSchedule.Group.Name,
                StudentFullName = attendance.Student.FullName,
                StudentNumber = attendance.Student.StudentNumber,
                TeacherFullName = attendance.LessonSchedule.Lesson.Teacher.FullName,
                Status = attendance.Status,
                IsLocked = attendance.IsLocked,
                MarkedAt = attendance.MarkedAt,
                LockAt = attendance.LockAt,
                AuditLogs = attendance.AuditLogs
                    .OrderByDescending(a => a.ChangedAt)
                    .Select(a => new AttendanceAuditLogDto
                    {
                        Id = a.Id,
                        OldStatus = a.OldStatus,
                        NewStatus = a.NewStatus,
                        WasLockedBeforeChange = a.WasLockedBeforeChange,
                        WasAdminOverride = a.WasAdminOverride,
                        ChangedAt = a.ChangedAt,
                        ChangedByUserId = a.ChangedByUserId,
                        Note = a.Note
                    })
                    .ToList()
            };
        }
    }
}
