using FluentValidation;

namespace Application.Modules.SubmissionsModule.Commands.SubmitAssignment
{
    public class SubmitAssignmentCommandValidator : AbstractValidator<SubmitAssignmentCommand>
    {
        public SubmitAssignmentCommandValidator()
        {
            RuleFor(x => x.AssignmentId).GreaterThan(0);
            RuleFor(x => x.StudentId).GreaterThan(0);
            RuleFor(x => x.Note).MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Note));
            // e.g., RuleFor(x => x.RepositoryLink).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.RepositoryLink));
        }
    }
}
