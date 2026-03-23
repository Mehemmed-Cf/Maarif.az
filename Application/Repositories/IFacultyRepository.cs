using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IFacultyRepository : IAsyncRepository<Faculty>
    {
        Task<Department?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    }
}