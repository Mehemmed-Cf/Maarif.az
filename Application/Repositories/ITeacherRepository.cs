using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface ITeacherRepository : IAsyncRepository<Teacher>
    {
        Task<Teacher?> GetByFinCodeAsync(string finCode, CancellationToken ct = default);
        Task<Teacher?> GetByDocumentSerialNumberAsync(string normalizedSerial, CancellationToken ct = default);
        Task<Teacher?> GetByTeacherNumberAsync(string teacherNumber, CancellationToken ct = default);
        Task<Teacher?> GetByUserIdWithDetailsAsync(int userId, CancellationToken ct = default);
    }
}