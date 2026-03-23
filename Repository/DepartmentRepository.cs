using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class DepartmentRepository : AsyncRepository<Department>, IDepartmentRepository
    {
        private readonly DataContext _context;

        public DepartmentRepository(DataContext db) : base(db)
        {
            _context = db;
        }

        public async Task<Department?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Departments
            .AsNoTracking()
            .Include(d => d.Faculty)
            .Include(d => d.Students)
            .Include(d => d.Groups)
            .Include(d => d.Subjects)
            .Include(d => d.TeacherDepartments)
                .ThenInclude(td => td.Teacher)
            .FirstOrDefaultAsync(d => d.Id == id && d.DeletedAt == null, ct);
        }
    }
}
