using FluentValidation;

namespace Application.Modules.StudentsModule.Commands.StudentRemoveCommand
{
    public class StudentRemoveRequestValidator : AbstractValidator<StudentRemoveRequest>
    {
        public StudentRemoveRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Düzgün tələbə seçin");
        }
    }
}