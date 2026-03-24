namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentTeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double Experience { get; set; }
        // FIX #4: Removed GroupCount and ActiveLessons.
        // These are [NotMapped] computed properties on Teacher — EF/ProjectTo cannot
        // translate them to SQL. If you need these counts in the UI, add a dedicated
        // Dapper query to TeachersModule (e.g. GetTeacherStatsByDepartmentQuery)
        // that aggregates them in a single SQL call.
    }
}