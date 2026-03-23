using FluentValidation;

namespace Application.Modules.FacultiesModule.Commands.FacultyAddCommand
{
    public class FacultyAddRequestValidator : AbstractValidator<FacultyAddRequest>
    {
        public FacultyAddRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad mütləqdir");
        }
    }
}
