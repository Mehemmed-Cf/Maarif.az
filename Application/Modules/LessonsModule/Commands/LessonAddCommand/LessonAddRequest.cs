using MediatR;

namespace Application.Modules.LessonsModule.Commands.LessonAddCommand
{
    public class LessonAddRequest : IRequest<LessonResponseDto>
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}