using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentGetByIdRequestHandler : IRequestHandler<DepartmentGetByIdRequest, DepartmentGetByIdResponseDto>
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentGetByIdRequestHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<DepartmentGetByIdResponseDto> Handle(DepartmentGetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await departmentRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Departament tapılmadı");

            return mapper.Map<DepartmentGetByIdResponseDto>(entity);
        }
    }
}
