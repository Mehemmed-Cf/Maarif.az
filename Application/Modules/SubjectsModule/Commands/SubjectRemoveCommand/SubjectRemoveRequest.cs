using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand
{
    public class SubjectRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}