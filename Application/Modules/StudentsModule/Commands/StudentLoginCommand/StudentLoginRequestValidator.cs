using FluentValidation;

namespace Application.Modules.StudentsModule.Commands.StudentLoginCommand
{
    public class StudentLoginRequestValidator : AbstractValidator<StudentLoginRequest>
    {
        public StudentLoginRequestValidator()
        {
            RuleFor(x => x.StudentNumber)
                .NotEmpty().WithMessage("Tələbə nömrəsi boş ola bilməz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifrə boş ola bilməz.");
        }
    }
}