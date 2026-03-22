using MediatR;

namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentGetByIdRequest : IRequest<DepartmentGetByIdResponseDto>
    {
        public int Id { get; set; }
    }
}
