using MediatR;

namespace Application.Modules.LessonsModule.Commands.LessonRemoveCommand
{
    public class LessonRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
