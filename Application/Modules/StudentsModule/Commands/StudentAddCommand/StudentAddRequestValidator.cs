using FluentValidation;

namespace Application.Modules.StudentsModule.Commands.StudentAddCommand
{
    public class StudentAddRequestValidator : AbstractValidator<StudentAddRequest>
    {
        public StudentAddRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad mütləqdir")
                .MaximumLength(300).WithMessage("Ad 300 simvoldan çox ola bilməz");

            RuleFor(x => x.FatherName)
                .MaximumLength(200).WithMessage("Ata adı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.FinCode)
            .NotEmpty().WithMessage("FinCode mütləqdir")
            .MaximumLength(7).WithMessage("FinCode 7 simvoldan çox ola bilməz");

            RuleFor(x => x.StudentNumber)
                .NotEmpty().WithMessage("Tələbə nömrəsi mütləqdir")
                .MaximumLength(50).WithMessage("Tələbə nömrəsi 50 simvoldan çox ola bilməz");

            RuleFor(x => x.MobileNumber)
                .MaximumLength(20).WithMessage("Telefon nömrəsi 20 simvoldan çox ola bilməz");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Doğum tarixi mütləqdir")
                .LessThan(DateTime.UtcNow).WithMessage("Doğum tarixi keçmiş olmalıdır");

            RuleFor(x => x.Year)
                .InclusiveBetween((byte)1, (byte)6).WithMessage("Kurs 1 ilə 6 arasında olmalıdır");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Düzgün departament seçin");

            //RuleFor(x => x.UserId)
            //    .GreaterThan(0).WithMessage("Düzgün istifadəçi seçin");
        }
    }
}