using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherRemoveCommand
{
    public class TeacherRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}