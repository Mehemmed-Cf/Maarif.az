using FluentValidation;

namespace Application.Modules.TeachersModule.Commands.TeacherRemoveCommand
{
    public class TeacherRemoveRequestValidator : AbstractValidator<TeacherRemoveRequest>
    {
        public TeacherRemoveRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Düzgün Teacher seçin");
        }
    }
}