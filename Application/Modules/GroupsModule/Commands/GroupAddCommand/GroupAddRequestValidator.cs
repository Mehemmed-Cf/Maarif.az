using FluentValidation;

namespace Application.Modules.GroupsModule.Commands.GroupAddCommand
{
    public class GroupAddRequestValidator : AbstractValidator<GroupAddRequest>
    {
        public GroupAddRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Group name is required.")
                .MaximumLength(100).WithMessage("Group name cannot exceed 100 characters.");

            RuleFor(x => x.Year)
                .InclusiveBetween((byte)1, (byte)6)
                .WithMessage("Year must be between 1 and 6.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("DepartmentId must be a valid positive number.");
        }
    }
}
