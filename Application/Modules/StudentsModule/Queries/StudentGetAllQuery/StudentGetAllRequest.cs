using MediatR;

namespace Application.Modules.StudentsModule.Queries.StudentGetAllQuery
{
    public class StudentGetAllRequest : IRequest<IEnumerable<StudentGetAllResponseDto>>
    {
    }
}