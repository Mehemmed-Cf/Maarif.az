using Application.Modules.AttendanceModule;
using Application.Modules.AttendanceModule.Commands.AdminEditAttendanceCommand;

namespace Presentation.AppCode.ViewModels
{
    public class AdminAttendanceEditViewModel
    {
        public AttendanceDetailsDto Attendance { get; set; }
        public AdminEditAttendanceRequest Form { get; set; }
    }
}
