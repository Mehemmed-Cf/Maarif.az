using FluentValidation;

namespace Application.Modules.SubjectsModule.Commands.SubjectAddCommand
{
    public class SubjectAddRequestValidator : AbstractValidator<SubjectAddRequest>
    {
        public SubjectAddRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad mütləqdir")
                .MaximumLength(200).WithMessage("Ad 200 simvoldan çox ola bilməz");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Düzgün departament seçin");

            RuleFor(x => x.Term)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Term));

            RuleFor(x => x.GroupName)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.GroupName));

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
