using Application.Modules.DepartmentsModule.Commands.DepartmentsRemoveCommand;
using FluentValidation;
namespace Application.Modules.DepartmentsModule.Commands.DepartmentRemoveCommand
{
    public class DepartmentRemoveRequestValidator : AbstractValidator<DepartmentRemoveRequest>
    {
        public DepartmentRemoveRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Düzgün departament seçin");
        }
    }
}