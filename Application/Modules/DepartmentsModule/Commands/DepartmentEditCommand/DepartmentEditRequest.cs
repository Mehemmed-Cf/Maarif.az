using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand
{
    public class DepartmentEditRequest : IRequest<DepartmentEditResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FacultyId { get; set; }
    }
}
