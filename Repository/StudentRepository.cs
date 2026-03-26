using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class StudentRepository : AsyncRepository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext db) : base(db)
        {
        }

        public async Task<Student?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            // AsNoTracking — read-only query, no change tracking overhead.
            // HasQueryFilter on StudentConfiguration handles soft-delete automatically.
            // ThenInclude Faculty so the UI can show "Department → Faculty" breadcrumb
            // without a second round-trip.
            return await db.Set<Student>()
                .AsNoTracking()
                .Include(s => s.Department)
                    .ThenInclude(d => d.Faculty)
                .Include(s => s.StudentGroups)
                    .ThenInclude(sg => sg.Group)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }
    }
}