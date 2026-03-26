using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditRequest : IRequest<SubjectEditResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}