using Application.Modules.GroupsModule;
using Application.Modules.GroupsModule.Commands.GroupEditCommand;
using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class GroupRepository : AsyncRepository<Group>, IGroupRepository
    {
        private readonly IMapper mapper;
        private readonly DataContext _context;

        public GroupRepository(IMapper mapper, DataContext db) : base(db)
        {
            this.mapper = mapper;
            _context = db;
        }

        public async Task<Group> GetAsync(
            Expression<Func<Group, bool>>? expression = null,
            CancellationToken cancellationToken = default,
            Func<IQueryable<Group>, IQueryable<Group>>? include = null)
        {
            var query = _context.Set<Group>().AsQueryable();

            if (include != null)
                query = include(query);

            var entity = expression is not null
                ? await query.FirstOrDefaultAsync(expression, cancellationToken)
                : await query.FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                throw new DirectoryNotFoundException($"Group not found");

            return entity;
        }

        public async Task UpdateGroupAsync(GroupEditRequest request, CancellationToken ct = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            request.StudentIds ??= new List<int>();
            request.LessonIds ??= new List<int>();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM StudentGroups WHERE GroupId = {0}", request.Id);
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM LessonGroups WHERE GroupId = {0}", request.Id);

                    var newStudentGroups = request.StudentIds
                        .Distinct()
                        .Select(id => new StudentGroup { GroupId = request.Id, StudentId = id })
                        .ToList();

                    var newLessonGroups = request.LessonIds
                        .Distinct()
                        .Select(id => new LessonGroup { GroupId = request.Id, LessonId = id })
                        .ToList();

                    await _context.Set<StudentGroup>().AddRangeAsync(newStudentGroups, ct);
                    await _context.Set<LessonGroup>().AddRangeAsync(newLessonGroups, ct);

                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Groups SET Name = {0}, Year = {1}, DepartmentId = {2} WHERE Id = {3}",
                        request.Name, request.Year, request.DepartmentId, request.Id);

                    await _context.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            });
        }

        // GetAll — projection-only, no student details, StudentCount via Count()
        public async Task<IReadOnlyList<GroupResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<Group>()
                .AsNoTracking()
                .Include(g => g.Department)
                .Include(g => g.LessonGroups)
                .Include(g => g.StudentGroups)
                .ProjectTo<GroupResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);
            //.Select(g => new GroupResponseDto
            //{
            //    Id = g.Id,
            //    Name = g.Name,
            //    Year = g.Year,
            //    DepartmentName = g.Department.Name,
            //    StudentCount = g.StudentGroups.Count()
            //})
            //.ToListAsync(ct);
        }

        // GetById — full details with students via StudentGroups → Student
        public async Task<GroupDetailsResponseDto?> GetDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<Group>()
                .AsNoTracking()
                .Include(g => g.Department)
                .Include(g => g.StudentGroups)
                    .ThenInclude(sg => sg.Student)
                .Include(g => g.LessonGroups)                  // ✅ add this
                    .ThenInclude(lg => lg.Lesson)              // ✅ and this
                        .ThenInclude(l => l.Subject)           // ✅ for display name
                .Include(g => g.LessonGroups)
                    .ThenInclude(lg => lg.Lesson)
                        .ThenInclude(l => l.Teacher)           // ✅ for display name
                .Where(g => g.Id == id)
                .Select(g => new GroupDetailsResponseDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Year = g.Year,
                    DepartmentName = g.Department.Name,
                    DepartmentId = g.Department.Id,
                    Students = g.StudentGroups
                        .Select(sg => new StudentSmallDto
                        {
                            Id = sg.Student.Id,
                            FullName = sg.Student.FullName,
                            StudentNumber = sg.Student.StudentNumber
                        }).ToList(),
                    Lessons = g.LessonGroups                   // ✅ add this
                        .Select(lg => new LessonSmallDto
                        {
                            Id = lg.Lesson.Id,
                            Name = $"{lg.Lesson.Subject.Name} - {lg.Lesson.Teacher.FullName}"
                        }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
