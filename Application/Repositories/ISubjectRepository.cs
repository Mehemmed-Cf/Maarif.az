using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface ISubjectRepository : IAsyncRepository<Subject>
    {
        // GetAll() inherited — used by GetAll handler via ProjectTo.
        // GetByIdWithDetailsAsync loads Subject + Department + Lessons for the GetById handler.
        Task<Subject?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);

        /// <summary>Tracked load with department, lessons, topics, materials, and literature (for edit/remove flows).</summary>
        Task<Subject?> GetByIdWithDetailsTrackedAsync(int id, CancellationToken ct = default);
    }
}