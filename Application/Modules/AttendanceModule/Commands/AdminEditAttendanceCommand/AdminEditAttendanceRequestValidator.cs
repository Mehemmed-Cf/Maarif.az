using FluentValidation;

namespace Application.Modules.AttendanceModule.Commands.AdminEditAttendanceCommand
{
    public class AdminEditAttendanceRequestValidator : AbstractValidator<AdminEditAttendanceRequest>
    {
        public AdminEditAttendanceRequestValidator()
        {
            RuleFor(x => x.AttendanceId)
                .GreaterThan(0);

            // UserId is set from the signed-in admin inside the POST action after automatic validation runs.

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.Note)
                .MaximumLength(500);
        }
    }
}
