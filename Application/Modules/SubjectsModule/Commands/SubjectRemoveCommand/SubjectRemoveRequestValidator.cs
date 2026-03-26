using FluentValidation;

namespace Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand
{
    public class SubjectRemoveRequestValidator : AbstractValidator<SubjectRemoveRequest>
    {
        public SubjectRemoveRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Düzgün fənn seçin");
        }
    }
}