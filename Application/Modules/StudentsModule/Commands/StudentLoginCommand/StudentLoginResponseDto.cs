// Application/Modules/StudentsModule/Commands/StudentLoginCommand/StudentLoginResponseDto.cs
namespace Application.Modules.StudentsModule.Commands.StudentLoginCommand
{
    public class StudentLoginResponseDto
    {
        public string StudentNumber { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        public StudentLoginResponseDto() { }

        public StudentLoginResponseDto(string studentNumber, string fullName, string role)
        {
            StudentNumber = studentNumber;
            FullName = fullName;
            Role = role;
        }
    }
}