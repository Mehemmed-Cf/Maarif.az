using Application.Repositories;
using Domain.Models.Entities;
using MediatR;
using System.Globalization;

namespace Application.Modules.TeachersModule.Queries.GetTeacherPortalProfileQuery
{
    public class GetTeacherPortalProfileRequestHandler
        : IRequestHandler<GetTeacherPortalProfileRequest, TeacherPortalProfileDto?>
    {
        private readonly ITeacherRepository teacherRepository;

        public GetTeacherPortalProfileRequestHandler(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }

        public async Task<TeacherPortalProfileDto?> Handle(
            GetTeacherPortalProfileRequest request,
            CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(
                request.UserId,
                cancellationToken);

            if (teacher is null)
                return null;

            var deptNames = teacher.TeacherDepartments?
                .Select(td => td.Department?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n!)
                .Distinct()
                .ToList() ?? new List<string>();

            var lessonCount = teacher.Lessons?.Count ?? 0;
            var groupCount = teacher.Lessons?
                .SelectMany(l => l.LessonGroups ?? Enumerable.Empty<LessonGroup>())
                .Select(lg => lg.GroupId)
                .Distinct()
                .Count() ?? 0;

            return new TeacherPortalProfileDto
            {
                FullName = teacher.FullName ?? "",
                TeacherNumber = teacher.TeacherNumber ?? "—",
                Email = string.IsNullOrWhiteSpace(teacher.Email) ? "—" : teacher.Email.Trim(),
                MobileNumber = string.IsNullOrWhiteSpace(teacher.MobileNumber)
                    ? "—"
                    : teacher.MobileNumber.Trim(),
                BirthDateDisplay = teacher.BirthDate == default || teacher.BirthDate.Year < 1920
                    ? "—"
                    : teacher.BirthDate.ToString("dd.MM.yyyy"),
                ExperienceDisplay = FormatExperience(teacher.Experience),
                DepartmentsDisplay = deptNames.Count > 0 ? string.Join(", ", deptNames) : "—",
                ActiveLessonCount = lessonCount,
                DistinctGroupCount = groupCount
            };
        }

        private static string FormatExperience(double years)
        {
            if (years <= 0)
                return "—";
            return string.Format(CultureInfo.GetCultureInfo("az-Latn-AZ"), "{0:0.#} il staj", years);
        }
    }
}
