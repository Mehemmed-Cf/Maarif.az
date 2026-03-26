using MediatR;

namespace Application.Modules.StudentsModule.Commands.StudentRemoveCommand
{
    public class StudentRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}