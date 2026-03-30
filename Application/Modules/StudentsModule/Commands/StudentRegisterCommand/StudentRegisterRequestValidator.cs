using Domain.Models.ValueObjects;
using FluentValidation;

namespace Application.Modules.StudentsModule.Commands.StudentRegisterCommand
{
    public class StudentRegisterRequestValidator : AbstractValidator<StudentRegisterRequest>
    {
        public StudentRegisterRequestValidator()
        {
            RuleFor(x => x.SerialNumber)
           .NotEmpty().WithMessage("Seriya nömrəsi boş ola bilməz.")
           .MaximumLength(20).WithMessage("Seriya nömrəsi çox uzundur.");

            RuleFor(x => x.FinCode)
                .NotEmpty().WithMessage("FIN kod boş ola bilməz.")
                .Must(BeValidFinCode).WithMessage(
                    "FIN kod 7 simvoldan ibarət olmalı, yalnız hərf və rəqəm ehtiva etməlidir.");
        }

        private static bool BeValidFinCode(string value)
        {
            try { FinCode.Create(value); return true; }
            catch { return false; }
        }
    }
}
