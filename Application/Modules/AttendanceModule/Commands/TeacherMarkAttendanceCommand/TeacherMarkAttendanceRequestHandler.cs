using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Application.Modules.AttendanceModule.Commands.TeacherMarkAttendanceCommand
{
    public class TeacherMarkAttendanceRequestHandler : IRequestHandler<TeacherMarkAttendanceRequest, AttendanceSessionDetailsDto>
    {
        private static readonly TimeSpan ProductionAttendanceLockWindow = TimeSpan.FromHours(3);
        private static readonly TimeSpan DevelopmentAttendanceLockWindow = TimeSpan.FromDays(3650);

        private readonly ITeacherRepository teacherRepository;
        private readonly IAttendanceRepository attendanceRepository;
        private readonly IHostEnvironment hostEnvironment;

        public TeacherMarkAttendanceRequestHandler(
            ITeacherRepository teacherRepository,
            IAttendanceRepository attendanceRepository,
            IHostEnvironment hostEnvironment)
        {
            this.teacherRepository = teacherRepository;
            this.attendanceRepository = attendanceRepository;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<AttendanceSessionDetailsDto> Handle(TeacherMarkAttendanceRequest request, CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("Teacher account was not found.");

            if (!hostEnvironment.IsDevelopment())
                await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);

            var attendanceLockWindow = hostEnvironment.IsDevelopment()
                ? DevelopmentAttendanceLockWindow
                : ProductionAttendanceLockWindow;

            var schedule = await attendanceRepository.GetScheduleForTeacherAsync(teacher.Id, request.LessonScheduleId, cancellationToken)
                ?? throw new NotFoundException("Lesson schedule was not found for this teacher.");

            var sessionDate = request.SessionDate.Date;
            var expectedStudentIds = schedule.Group.StudentGroups
                .Select(sg => sg.StudentId)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            var requestedStudentIds = request.Students
                .Select(s => s.StudentId)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            if (!expectedStudentIds.SequenceEqual(requestedStudentIds))
                throw new BadRequestException("Attendance must be submitted for the full lesson group.");

            var existingRecords = await attendanceRepository.GetByLessonSessionAsync(schedule.Id, sessionDate, cancellationToken);

            if (existingRecords.Any(a => a.IsLocked) && !hostEnvironment.IsDevelopment())
                throw new ConflictException("This attendance session is locked and can no longer be edited by the teacher.");

            var now = DateTime.UtcNow;

            foreach (var input in request.Students)
            {
                var existing = existingRecords.FirstOrDefault(a => a.StudentId == input.StudentId);

                if (existing is null)
                {
                    existing = new Attendance
                    {
                        StudentId = input.StudentId,
                        LessonScheduleId = schedule.Id,
                        SessionDate = sessionDate,
                        Status = input.Status,
                        MarkedAt = now,
                        LockAt = now.Add(attendanceLockWindow),
                        IsLocked = false,
                        MarkedByTeacherId = teacher.Id
                    };

                    await attendanceRepository.AddAsync(existing, cancellationToken);
                }
                else
                {
                    existing.Status = input.Status;
                    existing.MarkedByTeacherId = teacher.Id;
                    if (hostEnvironment.IsDevelopment())
                    {
                        existing.IsLocked = false;
                        existing.LockAt = now.Add(attendanceLockWindow);
                    }

                    await attendanceRepository.EditAsync(existing);
                }
            }

            await attendanceRepository.SaveAsync(cancellationToken);

            return await attendanceRepository.GetTeacherSessionDetailsAsync(teacher.Id, schedule.Id, sessionDate, cancellationToken)
                ?? throw new NotFoundException("Attendance session was not found after saving.");
        }
    }
}
