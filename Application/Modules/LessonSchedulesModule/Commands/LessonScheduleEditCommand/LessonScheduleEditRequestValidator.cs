using FluentValidation;

namespace Application.Modules.LessonSchedulesModule.Commands.LessonScheduleEditCommand
{
    public class LessonScheduleEditRequestValidator : AbstractValidator<LessonScheduleEditRequest>
    {
        public LessonScheduleEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Schedule ID is required.");
                
            RuleFor(x => x.LessonId)
                .NotEmpty().WithMessage("Lesson is required.");

            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("Room is required.");

            RuleFor(x => x.DayOfWeek)
                .IsInEnum().WithMessage("Invalid Day of Week.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");
        }
    }
}