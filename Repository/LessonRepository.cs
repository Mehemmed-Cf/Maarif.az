using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Application.Modules.LessonsModule;
using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class LessonRepository : AsyncRepository<Lesson>, ILessonRepository
    {
        private readonly DataContext _context;

        public LessonRepository(DataContext db) : base(db)
        {
            _context = db;
        }

        // GetAll — Retrieves summary of lessons with teacher and subject info
        public async Task<IReadOnlyList<LessonResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<Lesson>()
                .AsNoTracking()
                .Select(l => new LessonResponseDto
                {
                    Id = l.Id,
                    SubjectName = l.Subject.Name,
                    TeacherFullName = l.Teacher.FullName,
                    // Count how many groups are assigned to this mixed lesson
                    GroupCount = l.LessonGroups.Count()
                })
                .ToListAsync(ct);
        }

        // GetById — Retrieves full lesson details including the list of mixed groups
        public async Task<LessonDetailResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<Lesson>()
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new LessonDetailResponseDto
                {
                    Id = l.Id,
                    SubjectName = l.Subject.Name,
                    TeacherFullName = l.Teacher.FullName,
                    // Map the many-to-many relationship (Lesson -> LessonGroups -> Group)
                    Groups = l.LessonGroups.Select(lg => new GroupResponseDto
                    {
                        Id = lg.Group.Id,
                        Name = lg.Group.Name,
                        Year = lg.Group.Year,
                        DepartmentName = lg.Group.Department.Name,
                        // Shows how many students are in each group attending the lesson
                        StudentCount = lg.Group.StudentGroups.Count()
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}