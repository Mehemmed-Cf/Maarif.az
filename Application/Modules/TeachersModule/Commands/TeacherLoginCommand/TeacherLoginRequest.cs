using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherLoginCommand
{
    public class TeacherLoginRequest : IRequest<TeacherLoginResponseDto>
    {
        public string TeacherNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
