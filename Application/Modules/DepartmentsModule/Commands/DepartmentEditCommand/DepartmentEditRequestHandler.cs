using Application.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand
{
    public class DepartmentEditRequestHandler : IRequestHandler<DepartmentEditRequest, DepartmentEditResponseDto>
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentEditRequestHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }
        public async Task<DepartmentEditResponseDto> Handle(DepartmentEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await departmentRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            entity.Name = request.Name;
            entity.FacultyId = request.FacultyId;
            entity.LastModifiedAt = DateTime.UtcNow;

            await departmentRepository.EditAsync(entity);
            await departmentRepository.SaveAsync(cancellationToken);

            return mapper.Map<DepartmentEditResponseDto>(entity);
        }
    }
}
