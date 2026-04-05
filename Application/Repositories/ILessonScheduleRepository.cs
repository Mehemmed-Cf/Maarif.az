using Domain.Models.Entities;
using Domain.Models.Stables;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface ILessonScheduleRepository : IAsyncRepository<LessonSchedule>
    {
        Task<IReadOnlyList<LessonSchedule>> GetByGroupAsync(
            int groupId, WeekType? filter, CancellationToken ct = default);

        Task<IReadOnlyList<LessonSchedule>> GetByTeacherAsync(
            int teacherId, WeekType? filter, CancellationToken ct = default);

        Task<IReadOnlyList<LessonSchedule>> GetAllWithIncludesAsync(CancellationToken ct = default);

        Task<LessonSchedule?> GetByIdWithIncludesAsync(int id, CancellationToken ct = default);
    }
}
