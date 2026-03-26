using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.StudentsModule.Queries.StudentGetAllQuery
{
    public class StudentGetAllRequestHandler : IRequestHandler<StudentGetAllRequest, IEnumerable<StudentGetAllResponseDto>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentGetAllRequestHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StudentGetAllResponseDto>> Handle(
            StudentGetAllRequest request,
            CancellationToken cancellationToken)
        {
            // ProjectTo translates the entire mapping — including
            // Department.Name and Department.Faculty.Name — into a single SQL
            // query with no in-memory collection loading.
            // HasQueryFilter handles soft-delete automatically.
            return await studentRepository
                .GetAll()
                .ProjectTo<StudentGetAllResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}