using Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand;
using FluentValidation;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand
{
    public class DepartmentAddRequestValidator : AbstractValidator<DepartmentAddRequest>
    {
        public DepartmentAddRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad mütləqdir");
            RuleFor(x => x.FacultyId).GreaterThan(0).WithMessage("Düzgün fakültə seçin");
        }
    }
}
