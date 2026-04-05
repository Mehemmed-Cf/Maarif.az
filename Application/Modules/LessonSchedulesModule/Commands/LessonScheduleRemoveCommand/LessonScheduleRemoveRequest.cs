using MediatR;

namespace Application.Modules.LessonSchedulesModule.Commands.LessonScheduleRemoveCommand
{
    public class LessonScheduleRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
