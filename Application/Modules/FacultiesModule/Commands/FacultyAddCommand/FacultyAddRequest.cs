using MediatR;

namespace Application.Modules.FacultiesModule.Commands.FacultyAddCommand
{
    public class FacultyAddRequest : IRequest<FacultyAddResponseDto>
    {
        public string Name { get; set; }
    }
}
