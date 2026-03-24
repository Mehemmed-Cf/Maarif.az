using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IFacultyRepository : IAsyncRepository<Faculty>
    {
        Task<Faculty?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    }
}