using FluentValidation;

namespace Application.Modules.SubjectsModule.Commands.SubjectAddCommand
{
    public class SubjectAddRequestValidator : AbstractValidator<SubjectAddRequest>
    {
        public SubjectAddRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad mütləqdir")
                .MaximumLength(200).WithMessage("Ad 200 simvoldan çox ola bilməz");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Düzgün departament seçin");
        }
    }
}