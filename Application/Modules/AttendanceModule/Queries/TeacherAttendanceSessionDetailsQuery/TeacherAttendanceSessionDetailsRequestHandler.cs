using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionDetailsQuery
{
    public class TeacherAttendanceSessionDetailsRequestHandler : IRequestHandler<TeacherAttendanceSessionDetailsRequest, AttendanceSessionDetailsDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IAttendanceRepository attendanceRepository;

        public TeacherAttendanceSessionDetailsRequestHandler(
            ITeacherRepository teacherRepository,
            IAttendanceRepository attendanceRepository)
        {
            this.teacherRepository = teacherRepository;
            this.attendanceRepository = attendanceRepository;
        }

        public async Task<AttendanceSessionDetailsDto> Handle(TeacherAttendanceSessionDetailsRequest request, CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("Teacher account was not found.");

            await attendanceRepository.SyncExpiredLocksAsync(cancellationToken);

            var result = await attendanceRepository.GetTeacherSessionDetailsAsync(
                teacher.Id,
                request.LessonScheduleId,
                request.SessionDate.Date,
                cancellationToken);

            if (result is not null)
            {
                result.IsTeacherOwner = true;
                return result;
            }

            var schedule = await attendanceRepository.GetScheduleForTeacherAsync(teacher.Id, request.LessonScheduleId, cancellationToken)
                ?? throw new NotFoundException("Lesson schedule was not found for this teacher.");

            return new AttendanceSessionDetailsDto
            {
                LessonScheduleId = schedule.Id,
                SessionDate = request.SessionDate.Date,
                SubjectName = schedule.Lesson.Subject.Name,
                GroupName = schedule.Group.Name,
                TeacherFullName = schedule.Lesson.Teacher.FullName,
                RoomDisplay = schedule.Room.Building?.Name is string buildingName && !string.IsNullOrWhiteSpace(buildingName)
                    ? $"{buildingName} · otaq {schedule.Room.Number}"
                    : $"otaq {schedule.Room.Number}",
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                IsTeacherOwner = true,
                IsSessionLocked = false,
                Students = schedule.Group.StudentGroups
                    .Select(sg => sg.Student)
                    .OrderBy(s => s.FullName)
                    .Select(s => new AttendanceListItemDto
                    {
                        Id = 0,
                        StudentId = s.Id,
                        StudentFullName = s.FullName,
                        StudentNumber = s.StudentNumber,
                        HasRecord = false,
                        Status = Domain.Models.Stables.AttendanceStatus.Absent,
                        MarkedAt = DateTime.MinValue,
                        LockAt = DateTime.MinValue,
                        IsLocked = false
                    })
                    .ToList()
            };
        }
    }
}
