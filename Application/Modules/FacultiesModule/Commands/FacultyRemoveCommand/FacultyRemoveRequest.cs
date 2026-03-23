using MediatR;

namespace Application.Modules.FacultiesModule.Commands.FacultyRemoveCommand
{
    public class FacultyRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
