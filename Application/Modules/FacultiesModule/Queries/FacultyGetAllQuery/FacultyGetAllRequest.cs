using MediatR;

namespace Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery
{
    public class FacultyGetAllRequest : IRequest<IEnumerable<FacultyGetAllResponseDto>>
    {
    }
}
