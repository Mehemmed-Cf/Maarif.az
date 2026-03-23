using FluentValidation;

namespace Application.Modules.FacultiesModule.Commands.FacultyEditCommand
{
    public class FacultyEditRequestValidator : AbstractValidator<FacultyEditRequest>
    {
        public FacultyEditRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad mütləqdir");
        }
    }
}
