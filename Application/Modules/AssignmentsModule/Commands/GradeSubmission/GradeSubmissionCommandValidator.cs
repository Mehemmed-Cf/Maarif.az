using FluentValidation;

namespace Application.Modules.AssignmentsModule.Commands.GradeSubmission
{
    public class GradeSubmissionCommandValidator : AbstractValidator<GradeSubmissionCommand>
    {
        public GradeSubmissionCommandValidator()
        {
            RuleFor(x => x.SubmissionId).GreaterThan(0);
            RuleFor(x => x.Grade).InclusiveBetween(0, 100);
        }
    }
}
