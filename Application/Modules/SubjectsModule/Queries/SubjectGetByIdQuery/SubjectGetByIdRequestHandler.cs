using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery
{
    public class SubjectGetByIdRequestHandler : IRequestHandler<SubjectGetByIdRequest, SubjectGetByIdResponseDto>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public SubjectGetByIdRequestHandler(ISubjectRepository subjectRepository, IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<SubjectGetByIdResponseDto> Handle(
            SubjectGetByIdRequest request,
            CancellationToken cancellationToken)
        {
            // GetByIdWithDetailsAsync loads Department → Faculty and Lessons → Teacher.
            // HasQueryFilter on SubjectConfiguration ensures soft-deleted subjects
            // are never returned.
            var entity = await subjectRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            return mapper.Map<SubjectGetByIdResponseDto>(entity);
        }
    }
}