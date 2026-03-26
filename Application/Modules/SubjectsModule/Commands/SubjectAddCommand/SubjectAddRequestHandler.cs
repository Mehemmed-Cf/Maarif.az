using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectAddCommand
{
    public class SubjectAddRequestHandler : IRequestHandler<SubjectAddRequest, SubjectAddResponseDto>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public SubjectAddRequestHandler(
            ISubjectRepository subjectRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<SubjectAddResponseDto> Handle(
            SubjectAddRequest request,
            CancellationToken cancellationToken)
        {
            // Verify department exists before creating the subject under it.
            _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);

            var entity = new Subject
            {
                Name = request.Name,
                DepartmentId = request.DepartmentId,
            };

            await subjectRepository.AddAsync(entity, cancellationToken);
            await subjectRepository.SaveAsync(cancellationToken);

            // Reload with Department so the response can include DepartmentName.
            var created = await subjectRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);
            return mapper.Map<SubjectAddResponseDto>(created);
        }
    }
}