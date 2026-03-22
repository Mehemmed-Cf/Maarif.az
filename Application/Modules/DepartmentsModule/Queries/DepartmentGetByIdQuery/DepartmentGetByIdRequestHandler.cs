using Application.Repositories;
using AutoMapper;
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
            var entity = await departmentRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if(entity == null)
                throw new Exception("Departament tapılmadı");

            return mapper.Map<DepartmentGetByIdResponseDto>(entity);
        }
    }
}
