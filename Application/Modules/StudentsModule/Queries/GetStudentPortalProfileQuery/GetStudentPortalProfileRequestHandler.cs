using Application.Repositories;
using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.StudentsModule.Queries.GetStudentPortalProfileQuery
{
    public class GetStudentPortalProfileRequestHandler
        : IRequestHandler<GetStudentPortalProfileRequest, StudentPortalProfileDto?>
    {
        private readonly IStudentRepository studentRepository;

        public GetStudentPortalProfileRequestHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task<StudentPortalProfileDto?> Handle(
            GetStudentPortalProfileRequest request,
            CancellationToken cancellationToken)
        {
            var student = await studentRepository.GetByUserIdWithDetailsAsync(
                request.UserId,
                cancellationToken);

            if (student is null)
                return null;

            var groups = student.StudentGroups?
                .Select(sg => sg.Group?.Name)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n!)
                .Distinct()
                .ToList() ?? new List<string>();

            return new StudentPortalProfileDto
            {
                FullName = student.FullName ?? "",
                StudentNumber = student.StudentNumber ?? "",
                FatherName = DisplayOrDash(student.FatherName),
                GenderDisplay = GenderAz(student.Gender),
                BirthDateDisplay = FormatBirthDate(student.BirthDate),
                MobileNumber = DisplayOrDash(student.MobileNumber),
                CourseYearDisplay = CourseYearAz(student.Year),
                StatusDisplay = StatusAz(student.Status),
                EducationTypeDisplay = EducationAz(student.EducationType),
                DepartmentName = student.Department?.Name ?? "—",
                FacultyName = student.Department?.Faculty?.Name ?? "—",
                GroupNames = groups.Count > 0 ? string.Join(", ", groups) : "—"
            };
        }

        private static string DisplayOrDash(string? value)
            => string.IsNullOrWhiteSpace(value) ? "—" : value.Trim();

        private static string FormatBirthDate(DateTime birthDate)
        {
            if (birthDate == default || birthDate.Year < 1920)
                return "—";
            return birthDate.ToString("dd.MM.yyyy");
        }

        private static string CourseYearAz(byte year) => year switch
        {
            1 => "I kurs",
            2 => "II kurs",
            3 => "III kurs",
            4 => "IV kurs",
            _ => $"{year}-ci kurs"
        };

        private static string GenderAz(GenderType g) => g switch
        {
            GenderType.Male => "Kişi",
            GenderType.Female => "Qadın",
            _ => "—"
        };

        private static string EducationAz(EducationType e) => e switch
        {
            EducationType.StateFunded => "Dövlət sifarişli",
            EducationType.Paid => "Ödənişli",
            _ => "—"
        };

        private static string StatusAz(StatusType s) => s switch
        {
            StatusType.Active => "Təhsil alır",
            StatusType.AcademicLeave => "Akademik məzuniyyət",
            StatusType.MedicalLeave => "Tibbi məzuniyyət",
            StatusType.Graduated => "Məzun",
            StatusType.Transferred => "Köçürülüb",
            StatusType.Expelled => "Xaric edilib",
            StatusType.DroppedOut => "Təhsildən çıxıb",
            _ => "—"
        };
    }
}
