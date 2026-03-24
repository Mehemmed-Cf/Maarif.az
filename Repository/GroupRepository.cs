using Application.Modules.GroupsModule;
using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GroupRepository : AsyncRepository<Group>, IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext db) : base(db)
        {
            _context = db;
        }

        // GetAll — projection-only, no student details, StudentCount via Count()
        public async Task<IReadOnlyList<GroupResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<Group>()
                .AsNoTracking()
                .Include(g => g.Department)
                .Include(g => g.StudentGroups)
                .Select(g => new GroupResponseDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Year = g.Year,
                    DepartmentName = g.Department.Name,
                    StudentCount = g.StudentGroups.Count()
                })
                .ToListAsync(ct);
        }

        // GetById — full details with students via StudentGroups → Student
        public async Task<GroupDetailsResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<Group>()
                .AsNoTracking()
                .Include(g => g.Department)
                .Include(g => g.StudentGroups)
                    .ThenInclude(sg => sg.Student)
                .Where(g => g.Id == id)
                .Select(g => new GroupDetailsResponseDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Year = g.Year,
                    DepartmentName = g.Department.Name,
                    Students = g.StudentGroups
                        .Select(sg => new StudentSmallDto
                        {
                            Id = sg.Student.Id,
                            FullName = sg.Student.FullName,
                            StudentNumber = sg.Student.StudentNumber
                        }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
