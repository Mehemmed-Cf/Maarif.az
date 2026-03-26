using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SubjectRepository : AsyncRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(DbContext db) : base(db)
        {
        }

        public async Task<Subject?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            // Includes Department (for FacultyName breadcrumb) and Lessons
            // so the GetById response can show the teacher/group count for this subject.
            return await db.Set<Subject>()
                .AsNoTracking()
                .Include(s => s.Department)
                    .ThenInclude(d => d.Faculty)
                .Include(s => s.Lessons)
                    .ThenInclude(l => l.Teacher)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }
    }
}