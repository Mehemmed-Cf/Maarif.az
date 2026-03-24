using FluentValidation;

namespace Application.Modules.LessonsModule.Commands.LessonRemoveCommand
{
    public class LessonRemoveRequestValidator : AbstractValidator<LessonRemoveRequest>
    {
        public LessonRemoveRequestValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a valid positive number.");
        }
    }
}
