namespace Application.Modules.TeachersModule.Commands.TeacherLoginCommand
{
    public class TeacherLoginResponseDto
    {
        public string TeacherNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public TeacherLoginResponseDto(string teacherNumber, string fullName, string role)
        {
            TeacherNumber = teacherNumber;
            FullName = fullName;
            Role = role;
        }
    }
}
