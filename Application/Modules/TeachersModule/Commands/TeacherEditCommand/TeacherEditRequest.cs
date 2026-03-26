using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherEditCommand
{
    public class TeacherEditRequest : IRequest<TeacherResponseDto>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public double Experience { get; set; }
        public string Email { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}