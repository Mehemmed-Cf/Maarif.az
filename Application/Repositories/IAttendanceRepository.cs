using Application.Modules.AttendanceModule;
using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IAttendanceRepository : IAsyncRepository<Attendance>
    {
        Task SyncExpiredLocksAsync(CancellationToken ct = default);
        Task<Domain.Models.Entities.LessonSchedule?> GetScheduleForTeacherAsync(int teacherId, int lessonScheduleId, CancellationToken ct = default);
        Task<Attendance?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
        Task<Attendance?> GetByUniqueKeyAsync(int lessonScheduleId, int studentId, DateTime sessionDate, CancellationToken ct = default);
        Task<IReadOnlyList<Attendance>> GetByLessonSessionAsync(int lessonScheduleId, DateTime sessionDate, CancellationToken ct = default);
        Task<IReadOnlyList<AttendanceSessionListItemDto>> GetTeacherSessionsAsync(int teacherId, CancellationToken ct = default);
        Task<AttendanceSessionDetailsDto?> GetTeacherSessionDetailsAsync(int teacherId, int lessonScheduleId, DateTime sessionDate, CancellationToken ct = default);
        Task<IReadOnlyList<StudentAttendanceListItemDto>> GetStudentAttendanceAsync(int studentId, CancellationToken ct = default);
        Task<IReadOnlyList<AdminAttendanceListItemDto>> GetAdminAttendanceAsync(CancellationToken ct = default);
        Task<IReadOnlyList<AttendanceAudit>> GetAuditTrailAsync(int attendanceId, CancellationToken ct = default);
        Task AddAuditLogsAsync(IEnumerable<AttendanceAudit> auditLogs, CancellationToken ct = default);
    }
}
