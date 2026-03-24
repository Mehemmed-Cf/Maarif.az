using MediatR;

namespace Application.Modules.LessonsModule.Commands.LessonEditCommand
{
    public class LessonEditRequest : IRequest<LessonResponseDto>
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}
