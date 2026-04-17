using Application.Modules.SubjectsModule.Commands.SubjectEditCommand;
using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetForEditQuery
{
    public class SubjectGetForEditRequestHandler : IRequestHandler<SubjectGetForEditRequest, SubjectEditRequest>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public SubjectGetForEditRequestHandler(ISubjectRepository subjectRepository, IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<SubjectEditRequest> Handle(SubjectGetForEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await subjectRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            return mapper.Map<SubjectEditRequest>(entity);
        }
    }
}
