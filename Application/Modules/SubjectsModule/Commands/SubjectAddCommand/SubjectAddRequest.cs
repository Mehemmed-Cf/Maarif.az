using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectAddCommand
{
    public class SubjectAddRequest : IRequest<SubjectAddResponseDto>
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}