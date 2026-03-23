namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentTeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int GroupCount { get; set; }
        public int ActiveLessons { get; set; }
        public double Experience { get; set; }
    }
}
