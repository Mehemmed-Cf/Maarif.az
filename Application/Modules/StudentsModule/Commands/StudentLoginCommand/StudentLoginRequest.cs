// Application/Modules/StudentsModule/Commands/StudentLoginCommand/StudentLoginRequest.cs
using MediatR;

namespace Application.Modules.StudentsModule.Commands.StudentLoginCommand
{
    public class StudentLoginRequest : IRequest<StudentLoginResponseDto>
    {
        public string StudentNumber { get; set; }
        public string Password { get; set; }
    }
}