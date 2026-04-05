using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherRegisterCommand
{
    public class TeacherRegisterRequest : IRequest<TeacherRegisterResponseDto>
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string FinCode { get; set; } = string.Empty;
    }
}
