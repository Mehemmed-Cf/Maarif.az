using FluentValidation;

namespace Application.Modules.LessonsModule.Commands.LessonAddCommand
{
    public class LessonAddRequestValidator : AbstractValidator<LessonAddRequest>
    {
        public LessonAddRequestValidator()
        {
            RuleFor(x => x.TeacherId)
            .GreaterThan(0)
            .WithMessage("Teacher Id must be a valid positive number.");

            RuleFor(x => x.SubjectId)
            .GreaterThan(0)
            .WithMessage("Subject Id must be a valid positive number.");
        }
    }
}
