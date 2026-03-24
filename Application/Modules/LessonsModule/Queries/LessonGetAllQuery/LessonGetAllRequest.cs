using MediatR;

namespace Application.Modules.LessonsModule.Queries.LessonGetAllQuery
{
    public class LessonGetAllRequest : IRequest<IEnumerable<LessonResponseDto>>
    {
    }
}
