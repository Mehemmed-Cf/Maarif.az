using FluentValidation;

namespace Application.Modules.GroupsModule.Commands.GroupRemoveCommand
{
    public class GroupRemoveRequestValidator : AbstractValidator<GroupRemoveRequest>
    {
        public GroupRemoveRequestValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Group Id must be a valid positive number.");
        }
    }
}
