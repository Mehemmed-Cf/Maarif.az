using Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery;
using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.TeachersModule.Queries.TeacherGetAllQuery
{
    public class TeacherGetAllRequestHandler : IRequestHandler<TeacherGetAllRequest, IEnumerable<TeacherResponseDto>>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IMapper mapper;

        public TeacherGetAllRequestHandler(ITeacherRepository teacherRepository, IMapper mapper)
        {
            this.teacherRepository = teacherRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TeacherResponseDto>> Handle(TeacherGetAllRequest request, CancellationToken cancellationToken)
        {
            //var teachers = await teacherRepository.GetAll()
            //.Include(t => t.TeacherDepartments)
            //    .ThenInclude(td => td.Department)
            //    .ToListAsync(cancellationToken);

            var query = teacherRepository.GetAll();

            query = query.Where(m => m.DeletedAt == null);

            var result = await teacherRepository.GetAll()
                .Where(m => m.DeletedAt == null)
                .ProjectTo<TeacherResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
