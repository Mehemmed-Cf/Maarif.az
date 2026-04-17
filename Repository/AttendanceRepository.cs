using Application.Modules.AttendanceModule;
using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Domain.Models.Stables;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AttendanceRepository : AsyncRepository<Attendance>, IAttendanceRepository
    {
        private readonly DataContext context;

        public AttendanceRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task SyncExpiredLocksAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var expired = await context.Attendances
                .Where(a => !a.IsLocked && a.LockAt <= now)
                .ToListAsync(ct);

            if (expired.Count == 0)
                return;

            foreach (var item in expired)
                item.IsLocked = true;

            await context.SaveChangesAsync(ct);
        }

        public async Task<Domain.Models.Entities.LessonSchedule?> GetScheduleForTeacherAsync(int teacherId, int lessonScheduleId, CancellationToken ct = default)
        {
            return await context.LessonSchedules
                .Include(ls => ls.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(ls => ls.Lesson)
                    .ThenInclude(l => l.Teacher)
                .Include(ls => ls.Group)
                    .ThenInclude(g => g.StudentGroups)
                        .ThenInclude(sg => sg.Student)
                .Include(ls => ls.Room)
                    .ThenInclude(r => r.Building)
                .FirstOrDefaultAsync(ls => ls.Id == lessonScheduleId && ls.Lesson.TeacherId == teacherId, ct);
        }

        public async Task<Attendance?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await context.Attendances
                .AsNoTracking()
                .Include(a => a.Student)
                .Include(a => a.LessonSchedule)
                    .ThenInclude(ls => ls.Group)
                .Include(a => a.LessonSchedule)
                    .ThenInclude(ls => ls.Lesson)
                        .ThenInclude(l => l.Subject)
                .Include(a => a.LessonSchedule)
                    .ThenInclude(ls => ls.Lesson)
                        .ThenInclude(l => l.Teacher)
                .Include(a => a.AuditLogs.OrderByDescending(al => al.ChangedAt))
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public async Task<Attendance?> GetByUniqueKeyAsync(int lessonScheduleId, int studentId, DateTime sessionDate, CancellationToken ct = default)
        {
            var normalizedDate = sessionDate.Date;
            return await context.Attendances
                .FirstOrDefaultAsync(
                    a => a.LessonScheduleId == lessonScheduleId &&
                         a.StudentId == studentId &&
                         a.SessionDate == normalizedDate,
                    ct);
        }

        public async Task<IReadOnlyList<Attendance>> GetByLessonSessionAsync(int lessonScheduleId, DateTime sessionDate, CancellationToken ct = default)
        {
            var normalizedDate = sessionDate.Date;

            return await context.Attendances
                .Include(a => a.Student)
                .Include(a => a.LessonSchedule)
                    .ThenInclude(ls => ls.Lesson)
                .Where(a => a.LessonScheduleId == lessonScheduleId && a.SessionDate == normalizedDate)
                .OrderBy(a => a.Student.FullName)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<AttendanceSessionListItemDto>> GetTeacherSessionsAsync(int teacherId, CancellationToken ct = default)
        {
            return await context.Attendances
                .AsNoTracking()
                .Where(a => a.LessonSchedule.Lesson.TeacherId == teacherId)
                .GroupBy(a => new
                {
                    a.LessonScheduleId,
                    a.SessionDate,
                    SubjectName = a.LessonSchedule.Lesson.Subject.Name,
                    GroupName = a.LessonSchedule.Group.Name,
                    BuildingName = a.LessonSchedule.Room.Building.Name,
                    RoomNumber = a.LessonSchedule.Room.Number,
                    a.LessonSchedule.DayOfWeek,
                    a.LessonSchedule.StartTime,
                    a.LessonSchedule.EndTime
                })
                .OrderByDescending(g => g.Key.SessionDate)
                .ThenBy(g => g.Key.StartTime)
                .Select(g => new AttendanceSessionListItemDto
                {
                    LessonScheduleId = g.Key.LessonScheduleId,
                    SessionDate = g.Key.SessionDate,
                    SubjectName = g.Key.SubjectName,
                    GroupName = g.Key.GroupName,
                    RoomDisplay = string.IsNullOrWhiteSpace(g.Key.BuildingName)
                        ? $"otaq {g.Key.RoomNumber}"
                        : $"{g.Key.BuildingName} · otaq {g.Key.RoomNumber}",
                    DayOfWeek = g.Key.DayOfWeek,
                    StartTime = g.Key.StartTime,
                    EndTime = g.Key.EndTime,
                    AttendanceCount = g.Count(),
                    PresentCount = g.Count(x => x.Status == AttendanceStatus.Present),
                    AbsentCount = g.Count(x => x.Status == AttendanceStatus.Absent),
                    HasLockedRecords = g.Any(x => x.IsLocked)
                })
                .ToListAsync(ct);
        }

        public async Task<AttendanceSessionDetailsDto?> GetTeacherSessionDetailsAsync(int teacherId, int lessonScheduleId, DateTime sessionDate, CancellationToken ct = default)
        {
            var normalizedDate = sessionDate.Date;
            var schedule = await GetScheduleForTeacherAsync(teacherId, lessonScheduleId, ct);

            if (schedule is null)
                return null;

            var records = await context.Attendances
                .AsNoTracking()
                .Include(a => a.Student)
                .Where(a => a.LessonScheduleId == lessonScheduleId &&
                            a.SessionDate == normalizedDate &&
                            a.LessonSchedule.Lesson.TeacherId == teacherId)
                .OrderBy(a => a.Student.FullName)
                .ToListAsync(ct);

            var recordsByStudentId = records.ToDictionary(a => a.StudentId);
            var buildingName = schedule.Room.Building?.Name;

            return new AttendanceSessionDetailsDto
            {
                LessonScheduleId = schedule.Id,
                SessionDate = normalizedDate,
                SubjectName = schedule.Lesson.Subject.Name,
                GroupName = schedule.Group.Name,
                TeacherFullName = schedule.Lesson.Teacher.FullName,
                RoomDisplay = string.IsNullOrWhiteSpace(buildingName)
                    ? $"otaq {schedule.Room.Number}"
                    : $"{buildingName} · otaq {schedule.Room.Number}",
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                IsTeacherOwner = true,
                IsSessionLocked = records.Any(a => a.IsLocked),
                SessionMarkedAt = records.Count > 0 ? records.Min(a => a.MarkedAt) : null,
                SessionLockAt = records.Count > 0 ? records.Min(a => a.LockAt) : null,
                Students = schedule.Group.StudentGroups
                    .Select(sg => sg.Student)
                    .GroupBy(s => s.Id)
                    .Select(g => g.First())
                    .OrderBy(s => s.FullName)
                    .Select(s =>
                    {
                        if (recordsByStudentId.TryGetValue(s.Id, out var attendance))
                        {
                            return new AttendanceListItemDto
                            {
                                Id = attendance.Id,
                                StudentId = attendance.StudentId,
                                StudentFullName = attendance.Student.FullName,
                                StudentNumber = attendance.Student.StudentNumber,
                                HasRecord = true,
                                Status = attendance.Status,
                                MarkedAt = attendance.MarkedAt,
                                LockAt = attendance.LockAt,
                                IsLocked = attendance.IsLocked
                            };
                        }

                        return new AttendanceListItemDto
                        {
                            Id = 0,
                            StudentId = s.Id,
                            StudentFullName = s.FullName,
                            StudentNumber = s.StudentNumber,
                            HasRecord = false,
                            Status = AttendanceStatus.Absent,
                            MarkedAt = DateTime.MinValue,
                            LockAt = DateTime.MinValue,
                            IsLocked = false
                        };
                    })
                    .ToList()
            };
        }

        public async Task<IReadOnlyList<StudentAttendanceListItemDto>> GetStudentAttendanceAsync(int studentId, CancellationToken ct = default)
        {
            return await context.Attendances
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.SessionDate)
                .ThenByDescending(a => a.LessonSchedule.StartTime)
                .Select(a => new StudentAttendanceListItemDto
                {
                    AttendanceId = a.Id,
                    LessonScheduleId = a.LessonScheduleId,
                    SessionDate = a.SessionDate,
                    SubjectName = a.LessonSchedule.Lesson.Subject.Name,
                    TeacherFullName = a.LessonSchedule.Lesson.Teacher.FullName,
                    GroupName = a.LessonSchedule.Group.Name,
                    Status = a.Status,
                    MarkedAt = a.MarkedAt,
                    LockAt = a.LockAt,
                    IsLocked = a.IsLocked
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<AdminAttendanceListItemDto>> GetAdminAttendanceAsync(CancellationToken ct = default)
        {
            return await context.Attendances
                .AsNoTracking()
                .OrderByDescending(a => a.SessionDate)
                .ThenBy(a => a.LessonSchedule.Group.Name)
                .ThenBy(a => a.Student.FullName)
                .Select(a => new AdminAttendanceListItemDto
                {
                    AttendanceId = a.Id,
                    LessonScheduleId = a.LessonScheduleId,
                    SessionDate = a.SessionDate,
                    SubjectName = a.LessonSchedule.Lesson.Subject.Name,
                    GroupName = a.LessonSchedule.Group.Name,
                    StudentFullName = a.Student.FullName,
                    StudentNumber = a.Student.StudentNumber,
                    TeacherFullName = a.LessonSchedule.Lesson.Teacher.FullName,
                    Status = a.Status,
                    IsLocked = a.IsLocked,
                    MarkedAt = a.MarkedAt,
                    LockAt = a.LockAt
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<AttendanceAudit>> GetAuditTrailAsync(int attendanceId, CancellationToken ct = default)
        {
            return await context.AttendanceAudits
                .AsNoTracking()
                .Where(a => a.AttendanceId == attendanceId)
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync(ct);
        }

        public async Task AddAuditLogsAsync(IEnumerable<AttendanceAudit> auditLogs, CancellationToken ct = default)
        {
            await context.AttendanceAudits.AddRangeAsync(auditLogs, ct);
        }
    }
}
