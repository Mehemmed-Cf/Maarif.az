using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IDepartmentRepository : IAsyncRepository<Department>
    {
        Task<Department?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
        Task<Department?> GetByNameAsync(string name, CancellationToken ct = default);
    }
}
