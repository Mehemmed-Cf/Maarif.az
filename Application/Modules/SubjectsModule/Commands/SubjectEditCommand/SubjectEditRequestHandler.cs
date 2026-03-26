using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditRequestHandler : IRequestHandler<SubjectEditRequest, SubjectEditResponseDto>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public SubjectEditRequestHandler(
            ISubjectRepository subjectRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<SubjectEditResponseDto> Handle(
            SubjectEditRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await subjectRepository.GetAsync(s => s.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            if (entity.DepartmentId != request.DepartmentId)
                _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);

            entity.Name = request.Name;
            entity.DepartmentId = request.DepartmentId;

            await subjectRepository.EditAsync(entity);
            await subjectRepository.SaveAsync(cancellationToken);

            // Reload with Department navigation so the response includes DepartmentName.
            var updated = await subjectRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);
            return mapper.Map<SubjectEditResponseDto>(updated);
        }
    }
}