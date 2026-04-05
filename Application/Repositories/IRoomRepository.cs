using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IRoomRepository : IAsyncRepository<Room>
    {
        Task<IReadOnlyList<Room>> GetByBuildingAsync(int buildingId, CancellationToken ct = default);
    }
}
