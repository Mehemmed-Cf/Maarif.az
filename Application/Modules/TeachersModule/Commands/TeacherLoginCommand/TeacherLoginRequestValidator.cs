using FluentValidation;

namespace Application.Modules.TeachersModule.Commands.TeacherLoginCommand
{
    public class TeacherLoginRequestValidator : AbstractValidator<TeacherLoginRequest>
    {
        public TeacherLoginRequestValidator()
        {
            RuleFor(x => x.TeacherNumber).NotEmpty().WithMessage("Müəllim nömrəsi tələb olunur.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifrə tələb olunur.");
        }
    }
}
