using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherAddCommand
{
    public class TeacherAddRequest : IRequest<TeacherResponseDto>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double Experience { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}