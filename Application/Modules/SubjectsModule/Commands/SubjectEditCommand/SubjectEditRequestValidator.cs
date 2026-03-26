using FluentValidation;

namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditRequestValidator : AbstractValidator<SubjectEditRequest>
    {
        public SubjectEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Düzgün fənn seçin");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad mütləqdir")
                .MaximumLength(200).WithMessage("Ad 200 simvoldan çox ola bilməz");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Düzgün departament seçin");
        }
    }
}