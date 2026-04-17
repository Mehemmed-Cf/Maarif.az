using Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Queries.PortalSubjectQuery
{
    public class GetPortalSubjectWorkspaceRequestHandler
        : IRequestHandler<GetPortalSubjectWorkspaceRequest, PortalSubjectWorkspaceDto>
    {
        private readonly IMediator mediator;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public GetPortalSubjectWorkspaceRequestHandler(
            IMediator mediator,
            ISubjectRepository subjectRepository,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<PortalSubjectWorkspaceDto> Handle(
            GetPortalSubjectWorkspaceRequest request,
            CancellationToken cancellationToken)
        {
            var nav = await mediator.Send(
                new GetPortalSubjectNavRequest
                {
                    UserId = request.UserId,
                    ForTeacher = request.ForTeacher
                },
                cancellationToken);

            if (!nav.Any(n => n.Id == request.SubjectId))
                throw new NotFoundException("Fənn tapılmadı və ya giriş icazəniz yoxdur.");

            var entity = await subjectRepository.GetByIdWithDetailsAsync(request.SubjectId, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.SubjectId})");

            var subjectDto = mapper.Map<SubjectGetByIdResponseDto>(entity);

            return new PortalSubjectWorkspaceDto
            {
                NavSubjects = nav,
                Subject = subjectDto
            };
        }
    }
}
