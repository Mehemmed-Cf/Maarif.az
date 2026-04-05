using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RoomRepository : AsyncRepository<Room>, IRoomRepository
    {
        private readonly DataContext _context;

        public RoomRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Room>> GetByBuildingAsync(
            int buildingId, CancellationToken ct = default)
        {
            return await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Building)
                .Where(r => r.BuildingId == buildingId && r.DeletedAt == null)
                .OrderBy(r => r.Number)
                .ToListAsync(ct);
        }
    }
}
