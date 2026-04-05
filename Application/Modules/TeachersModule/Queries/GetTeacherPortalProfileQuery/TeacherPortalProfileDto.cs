namespace Application.Modules.TeachersModule.Queries.GetTeacherPortalProfileQuery
{
    public class TeacherPortalProfileDto
    {
        public string FullName { get; set; } = "";
        public string TeacherNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string BirthDateDisplay { get; set; } = "";
        public string ExperienceDisplay { get; set; } = "";
        public string DepartmentsDisplay { get; set; } = "";
        public int ActiveLessonCount { get; set; }
        public int DistinctGroupCount { get; set; }
    }
}
