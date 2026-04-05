using FluentValidation;

namespace Application.Modules.LessonSchedulesModule.Commands.LessonScheduleRemoveCommand
{
    public class LessonScheduleRemoveValidator : AbstractValidator<LessonScheduleRemoveRequest>
    {
        public LessonScheduleRemoveValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Schedule ID is required.");
        }
    }
}
