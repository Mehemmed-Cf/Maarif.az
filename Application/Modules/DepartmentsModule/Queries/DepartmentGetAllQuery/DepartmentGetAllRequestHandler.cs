using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery
{
    public class DepartmentGetAllRequestHandler : IRequestHandler<DepartmentGetAllRequest, IEnumerable<DepartmentGetAllResponseDto>>
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentGetAllRequestHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentGetAllResponseDto>> Handle(DepartmentGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = departmentRepository.GetAll();

            query = query.Where(m => m.DeletedAt == null);

            var result = await query
                    .ProjectTo<DepartmentGetAllResponseDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            return mapper.Map<IEnumerable<DepartmentGetAllResponseDto>>(query);
        }
    }
}
