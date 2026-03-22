using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
using FluentValidation;
namespace Application.Modules.DepartmentsModule.Commands.DepartmentEditCommand
{
    public class DepartmentEditRequestValidator : AbstractValidator<DepartmentEditRequest>
    {
        public DepartmentEditRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Düzgün departament seçin");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad mütləqdir");
            RuleFor(x => x.FacultyId).GreaterThan(0).WithMessage("Düzgün fakültə seçin");
        }
    }
}