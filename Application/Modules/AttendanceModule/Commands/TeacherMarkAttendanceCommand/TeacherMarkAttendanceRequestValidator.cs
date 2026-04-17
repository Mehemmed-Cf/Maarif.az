using FluentValidation;

namespace Application.Modules.AttendanceModule.Commands.TeacherMarkAttendanceCommand
{
    public class TeacherMarkAttendanceRequestValidator : AbstractValidator<TeacherMarkAttendanceRequest>
    {
        public TeacherMarkAttendanceRequestValidator()
        {
            // UserId is assigned in the MVC action from the authenticated user after model binding;
            // automatic FluentValidation runs before the action, so UserId must not be validated here.

            RuleFor(x => x.LessonScheduleId)
                .GreaterThan(0);

            RuleFor(x => x.SessionDate)
                .NotEmpty();

            RuleFor(x => x.Students)
                .NotNull()
                .NotEmpty().WithMessage("At least one student attendance item is required.");

            RuleForEach(x => x.Students)
                .ChildRules(student =>
                {
                    student.RuleFor(s => s.StudentId)
                        .GreaterThan(0);

                    student.RuleFor(s => s.Status)
                        .IsInEnum();
                });
        }
    }
}
