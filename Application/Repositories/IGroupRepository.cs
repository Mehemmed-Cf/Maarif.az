using Application.Modules.GroupsModule;
using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IGroupRepository : IAsyncRepository<Group>
    {
        Task<IReadOnlyList<GroupResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<GroupDetailsResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default);
    }
}