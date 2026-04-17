using FluentValidation;

namespace Application.Modules.AssignmentsModule.Commands.CreateAssignment
{
    public class CreateAssignmentCommandValidator : AbstractValidator<CreateAssignmentCommand>
    {
        public CreateAssignmentCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.LessonId).GreaterThan(0);
            RuleFor(x => x.DueDate).GreaterThan(DateTime.Now);
        }
    }
}
