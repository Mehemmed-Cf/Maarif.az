using FluentValidation;

namespace Application.Modules.LessonsModule.Commands.LessonEditCommand
{
    public class LessonEditRequestValidator : AbstractValidator<LessonEditRequest>
    {
        public LessonEditRequestValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a valid positive number.");

            RuleFor(x => x.TeacherId)
            .GreaterThan(0)
            .WithMessage("Teacher Id must be a valid positive number.");

            RuleFor(x => x.SubjectId)
            .GreaterThan(0)
            .WithMessage("Sunject Id must be a valid positive number.");
        }
    }
}
