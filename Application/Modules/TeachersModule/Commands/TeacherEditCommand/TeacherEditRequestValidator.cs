using FluentValidation;

namespace Application.Modules.TeachersModule.Commands.TeacherEditCommand
{
    public class TeacherEditRequestValidator : AbstractValidator<TeacherEditRequest>
    {
        public TeacherEditRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.DepartmentIds)
                .NotNull().WithMessage("Departments list cannot be null")
                .Must(x => x.Any()).WithMessage("At least one department must be selected");

            RuleForEach(x => x.DepartmentIds)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0");
        }
    }
}
