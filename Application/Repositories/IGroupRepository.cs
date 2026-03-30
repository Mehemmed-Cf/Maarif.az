using Application.Modules.GroupsModule;
using Application.Modules.GroupsModule.Commands.GroupEditCommand;
using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Domain.Models.Entities;
using Infrastructure.Abstracts;
using System.Linq.Expressions;

namespace Application.Repositories
{
    public interface IGroupRepository : IAsyncRepository<Group>
    {
        Task<IReadOnlyList<GroupResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<Group> GetAsync(
            Expression<Func<Group, bool>>? expression = null,
            CancellationToken cancellationToken = default,
            Func<IQueryable<Group>, IQueryable<Group>>? include = null);
        Task<GroupDetailsResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default);
        Task UpdateGroupAsync(GroupEditRequest request, CancellationToken ct = default);
    }
}