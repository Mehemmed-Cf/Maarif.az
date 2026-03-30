using Application.Modules.StudentsModule;
using Application.Repositories;
using AutoMapper.QueryableExtensions;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Repository
{
    public class StudentRepository : AsyncRepository<Student>, IStudentRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public StudentRepository(DataContext db, IMapper mapper) : base(db)
        {
            context = db;
            this.mapper = mapper;
        }

        public async Task<Student?> GetByFinCodeAsync(
        string finCode, CancellationToken ct = default)
        => await context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.FinCode == finCode, ct);

            public async Task<Student?> GetByStudentNumberAsync(
                string studentNumber, CancellationToken ct = default)
                => await context.Students
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber, ct);

            public async Task<IReadOnlyList<StudentListDto>> GetAllAsync(
                CancellationToken ct = default)
                => await context.Students
                    .AsNoTracking()
                    .ProjectTo<StudentListDto>(mapper.ConfigurationProvider)
                    .ToListAsync(ct);

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