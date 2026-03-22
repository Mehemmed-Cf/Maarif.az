using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand
{
    public class DepartmentAddRequestHandler : IRequestHandler<DepartmentAddRequest, DepartmentAddResponseDto>
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentAddRequestHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<DepartmentAddResponseDto> Handle(DepartmentAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new Department
            {
                Name = request.Name,
                FacultyId = request.FacultyId,
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow,
            };

            await departmentRepository.AddAsync(entity);
            await departmentRepository.SaveAsync(cancellationToken);

            return mapper.Map<DepartmentAddResponseDto>(entity);
        }
    }
}
