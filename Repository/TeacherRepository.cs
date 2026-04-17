using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TeacherRepository : AsyncRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DbContext db) : base(db)
        {
        }

        public async Task<Teacher?> GetByFinCodeAsync(string finCode, CancellationToken ct = default)
            => await db.Set<Teacher>()
                .FirstOrDefaultAsync(t => t.FinCode == finCode, ct);

        public async Task<Teacher?> GetByDocumentSerialNumberAsync(string normalizedSerial, CancellationToken ct = default)
            => await db.Set<Teacher>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    t => t.DocumentSerialNumber != null && t.DocumentSerialNumber == normalizedSerial,
                    ct);

        public async Task<Teacher?> GetByTeacherNumberAsync(string teacherNumber, CancellationToken ct = default)
            => await db.Set<Teacher>()
                .FirstOrDefaultAsync(t => t.TeacherNumber == teacherNumber, ct);

        public async Task<Teacher?> GetByUserIdWithDetailsAsync(int userId, CancellationToken ct = default)
            => await db.Set<Teacher>()
                .AsNoTracking()
                .Include(t => t.TeacherDepartments)
                    .ThenInclude(td => td.Department)
                    .ThenInclude(d => d.Faculty)
                .Include(t => t.Lessons)
                    .ThenInclude(l => l.LessonGroups)
                .Include(t => t.Lessons)
                    .ThenInclude(l => l.Subject)
                .FirstOrDefaultAsync(t => t.UserId == userId, ct);
    }
}