using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface ITeacherRepository : IAsyncRepository<Teacher>
    {
        Task<Department?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    }
}