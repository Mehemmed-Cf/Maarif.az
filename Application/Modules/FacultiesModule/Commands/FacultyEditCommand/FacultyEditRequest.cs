using MediatR;

namespace Application.Modules.FacultiesModule.Commands.FacultyEditCommand
{
    public class FacultyEditRequest : IRequest<FacultyEditResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
