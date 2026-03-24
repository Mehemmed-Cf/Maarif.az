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

        public async Task<IEnumerable<DepartmentGetAllResponseDto>> Handle(
            DepartmentGetAllRequest request,
            CancellationToken cancellationToken)
        {
            // FIX #1: Previous code built a ProjectTo query into `result`, then threw
            // it away and returned mapper.Map(query) on a raw IQueryable — causing two
            // round-trips and an in-memory mapping of an unmaterialized sequence.
            // ProjectTo is the correct approach: it translates the entire mapping to SQL
            // so Students/Subjects/Groups counts become SQL COUNT subqueries, not
            // C# collection loads. HasQueryFilter handles soft-delete automatically.
            return await departmentRepository
                .GetAll()
                .ProjectTo<DepartmentGetAllResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}