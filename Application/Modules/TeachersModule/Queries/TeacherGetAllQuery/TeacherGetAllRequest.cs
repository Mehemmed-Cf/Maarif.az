using MediatR;

namespace Application.Modules.TeachersModule.Queries.TeacherGetAllQuery
{
    public class TeacherGetAllRequest : IRequest<IEnumerable<TeacherResponseDto>>
    {

    }
}
