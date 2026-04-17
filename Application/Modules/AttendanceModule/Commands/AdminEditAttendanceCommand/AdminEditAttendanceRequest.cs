using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.AttendanceModule.Commands.AdminEditAttendanceCommand
{
    public class AdminEditAttendanceRequest : IRequest<AttendanceDetailsDto>
    {
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public AttendanceStatus Status { get; set; }
        public string? Note { get; set; }
    }
}
