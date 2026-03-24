using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
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

        public async Task<DepartmentEditResponseDto> Handle(
            DepartmentEditRequest request,
            CancellationToken cancellationToken)
        {
            // FIX #5: Removed manual `&& m.DeletedAt == null` — HasQueryFilter on
            // DepartmentConfiguration already applies this globally to every EF query.
            // Adding it manually is inconsistent and misleads readers into thinking
            // the filter wouldn't apply otherwise.
            // Added NotFoundException instead of letting a null entity throw a NullReferenceException.
            var entity = await departmentRepository.GetAsync(m => m.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Departament tapılmadı (Id: {request.Id})");

            entity.Name = request.Name;
            entity.FacultyId = request.FacultyId;
            entity.LastModifiedAt = DateTime.UtcNow;

            await departmentRepository.EditAsync(entity);
            await departmentRepository.SaveAsync(cancellationToken);

            return mapper.Map<DepartmentEditResponseDto>(entity);
        }
    }
}