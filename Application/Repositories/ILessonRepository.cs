using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Application.Modules.LessonsModule;
using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface ILessonRepository : IAsyncRepository<Lesson>
    {
        Task<IReadOnlyList<LessonResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<LessonDetailResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default);
    }
}