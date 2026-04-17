using FluentValidation;

namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditRequestValidator : AbstractValidator<SubjectEditRequest>
    {
        public SubjectEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Düzgün fənn seçin");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad mütləqdir")
                .MaximumLength(200).WithMessage("Ad 200 simvoldan çox ola bilməz");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Düzgün departament seçin");

            RuleFor(x => x.Term)
                .NotEmpty().WithMessage("Term mütləqdir")
                .MaximumLength(50);

            RuleFor(x => x.GroupName)
                .NotEmpty().WithMessage("Qrup adı mütləqdir")
                .MaximumLength(100);

            RuleFor(x => x.StudentCount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Credits).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalHours).GreaterThanOrEqualTo(0);
            RuleFor(x => x.WeekCount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FreeWorkScore).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SeminarScore).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LabScore).GreaterThanOrEqualTo(0);
            RuleFor(x => x.AttendanceScore).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ExamScore).GreaterThanOrEqualTo(0);

            RuleForEach(x => x.Topics).ChildRules(topic =>
            {
                topic.When(t => !string.IsNullOrWhiteSpace(t.TopicName), () =>
                {
                    topic.RuleFor(t => t.TopicName).MaximumLength(200);
                    topic.RuleFor(t => t.WeekNumber).GreaterThanOrEqualTo(0);
                });
            });
        }
    }
}
