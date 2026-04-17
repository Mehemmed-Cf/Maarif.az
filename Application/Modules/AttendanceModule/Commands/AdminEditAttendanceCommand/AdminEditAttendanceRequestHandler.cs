using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Application.Modules.AttendanceModule.Commands.AdminEditAttendanceCommand
{
    public class AdminEditAttendanceRequestHandler : IRequestHandler<AdminEditAttendanceRequest, AttendanceDetailsDto>
    {
        private readonly IAttendanceRepository attendanceRepository;
        private readonly IHostEnvironment hostEnvironment;

        public AdminEditAttendanceRequestHandler(
            IAttendanceRepository attendanceRepository,
            IHostEnvironment hostEnvironment)
        {
            this.attendanceRepository = attendanceRepository;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<AttendanceDetailsDto> Handle(AdminEditAttendanceRequest request, CancellationToken cancellationToken)
        {
            if (!hostEnvironment.IsDevelopment())
                await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);

            var attendance = await attendanceRepository.GetAsync(a => a.Id == request.AttendanceId, cancellationToken)
                ?? throw new NotFoundException("Attendance record was not found.");

            if (attendance.Status != request.Status)
            {
                var auditLog = new AttendanceAudit
                {
                    AttendanceId = attendance.Id,
                    OldStatus = attendance.Status,
                    NewStatus = request.Status,
                    WasLockedBeforeChange = attendance.IsLocked,
                    WasAdminOverride = true,
                    ChangedAt = DateTime.UtcNow,
                    ChangedByUserId = request.UserId,
                    Note = request.Note
                };

                attendance.Status = request.Status;
                attendance.IsLocked = attendance.IsLocked || attendance.LockAt <= DateTime.UtcNow;

                await attendanceRepository.EditAsync(attendance);
                await attendanceRepository.AddAuditLogsAsync(new[] { auditLog }, cancellationToken);
                await attendanceRepository.SaveAsync(cancellationToken);
            }

            var refreshed = await attendanceRepository.GetByIdWithDetailsAsync(attendance.Id, cancellationToken)
                ?? throw new NotFoundException("Attendance record was not found after update.");

            return new AttendanceDetailsDto
            {
                Id = refreshed.Id,
                LessonScheduleId = refreshed.LessonScheduleId,
                SessionDate = refreshed.SessionDate,
                SubjectName = refreshed.LessonSchedule.Lesson.Subject.Name,
                GroupName = refreshed.LessonSchedule.Group.Name,
                StudentFullName = refreshed.Student.FullName,
                StudentNumber = refreshed.Student.StudentNumber,
                TeacherFullName = refreshed.LessonSchedule.Lesson.Teacher.FullName,
                Status = refreshed.Status,
                IsLocked = refreshed.IsLocked,
                MarkedAt = refreshed.MarkedAt,
                LockAt = refreshed.LockAt,
                AuditLogs = refreshed.AuditLogs
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
