using FluentValidation;

namespace Application.Modules.FacultiesModule.Commands.FacultyRemoveCommand
{
    public class FacultyRemoveRequestValidator : AbstractValidator<FacultyRemoveRequest>
    {
        public FacultyRemoveRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Düzgün faculty seçin");
        }
    }
}
