namespace Application.Modules.StudentsModule.Commands.StudentRegisterCommand
{
    public class StudentRegisterResponseDto
    {
        public string StudentNumber { get; set; }
        public string DefaultPassword { get; set; }

        public StudentRegisterResponseDto(string studentNumber, string defaultPassword)
        {
            StudentNumber = studentNumber;
            DefaultPassword = defaultPassword;
        }

        // Always keep an empty constructor if using AutoMapper
        public StudentRegisterResponseDto() { }
    }
}
