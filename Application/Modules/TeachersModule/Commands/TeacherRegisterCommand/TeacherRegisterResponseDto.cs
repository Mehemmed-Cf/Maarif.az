namespace Application.Modules.TeachersModule.Commands.TeacherRegisterCommand
{
    public class TeacherRegisterResponseDto
    {
        public string TeacherNumber { get; set; } = string.Empty;
        public string DefaultPassword { get; set; } = string.Empty;

        public TeacherRegisterResponseDto(string teacherNumber, string defaultPassword)
        {
            TeacherNumber = teacherNumber;
            DefaultPassword = defaultPassword;
        }

        public TeacherRegisterResponseDto()
        {
        }
    }
}
