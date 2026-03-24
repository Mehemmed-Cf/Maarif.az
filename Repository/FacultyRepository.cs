using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FacultyRepository : AsyncRepository<Faculty>, IFacultyRepository
    {
        private readonly DataContext _context;

        public FacultyRepository(DataContext db) : base(db)
        {
            _context = db;
        }

        public async Task<Faculty?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Faculties
            .AsNoTracking()
            .Include(f => f.Departments)
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt == null, ct);
        }
    }
}