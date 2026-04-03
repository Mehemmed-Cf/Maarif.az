using Application.Modules.StudentsModule;
using Domain.Models.Entities;
using Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Repositories
{
    public interface IStudentRepository : IAsyncRepository<Student>
    {
        // FIX: Removed DTO-returning methods (GetAllAsync / GetDetailsByIdAsync).
        // Repositories return entities — DTOs are the Application layer's concern.
        // GetAll() (inherited) feeds ProjectTo<T> in the GetAll handler.
        // GetByIdWithDetailsAsync loads the entity with navigations for the GetById handler.
        Task<Student?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
        Task<Student?> GetByFinCodeAsync(string finCode, CancellationToken ct = default);
        Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken ct = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}