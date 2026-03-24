using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherAddCommand
{
    public class TeacherAddRequest : IRequest<TeacherResponseDto>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}