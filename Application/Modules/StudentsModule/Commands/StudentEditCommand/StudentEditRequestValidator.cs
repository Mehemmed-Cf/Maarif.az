using FluentValidation;

namespace Application.Modules.StudentsModule.Commands.StudentEditCommand
{
    public class StudentEditRequestValidator : AbstractValidator<StudentEditRequest>
    {
        public StudentEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Düzgün tələbə seçin");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad mütləqdir")
                .MaximumLength(300).WithMessage("Ad 300 simvoldan çox ola bilməz");

            RuleFor(x => x.FatherName)
                .MaximumLength(200).WithMessage("Ata adı 200 simvoldan çox ola bilməz");

            RuleFor(x => x.FullName)
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
        }
    }
}