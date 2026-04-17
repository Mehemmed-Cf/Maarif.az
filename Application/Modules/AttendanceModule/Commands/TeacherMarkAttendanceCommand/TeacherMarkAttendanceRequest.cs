using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.AttendanceModule.Commands.TeacherMarkAttendanceCommand
{
    public class TeacherMarkAttendanceRequest : IRequest<AttendanceSessionDetailsDto>
    {
        public int UserId { get; set; }
        public int LessonScheduleId { get; set; }
        public DateTime SessionDate { get; set; }
        public List<TeacherAttendanceStudentInputDto> Students { get; set; } = new();
    }

    public class TeacherAttendanceStudentInputDto
    {
        public int StudentId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
