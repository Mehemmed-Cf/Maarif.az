using MediatR;

namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery
{
    public class DepartmentGetAllRequest : IRequest<IEnumerable<DepartmentGetAllResponseDto>>
    {
    }
}
