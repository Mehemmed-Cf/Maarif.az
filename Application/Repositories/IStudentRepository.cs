using Application.Modules.StudentsModule;
using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IStudentRepository : IAsyncRepository<Student>
    {
        Task<IReadOnlyList<StudentDetailsResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<StudentDetailsResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default);
    }
}