using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery
{
    public class SubjectGetAllRequestHandler : IRequestHandler<SubjectGetAllRequest, IEnumerable<SubjectGetAllResponseDto>>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public SubjectGetAllRequestHandler(ISubjectRepository subjectRepository, IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SubjectGetAllResponseDto>> Handle(
            SubjectGetAllRequest request,
            CancellationToken cancellationToken)
        {
            // ProjectTo translates Department.Name, Department.Faculty.Name,
            // and Lessons.Count into a single SQL query — no in-memory loading.
            return await subjectRepository
                .GetAll()
                .ProjectTo<SubjectGetAllResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}