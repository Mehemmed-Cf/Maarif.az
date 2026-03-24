using MediatR;

namespace Application.Modules.LessonsModule.Queries.LessonGetByIdQuery
{
    public class LessonGetByIdRequest : IRequest<LessonDetailResponseDto>
    {
        public int Id { get; set; }
    }
}
