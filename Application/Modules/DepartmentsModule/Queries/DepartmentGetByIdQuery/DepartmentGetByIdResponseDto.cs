namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentGetByIdResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FacultyName { get; set; }
        public int StudentCount { get; set; }
        public int GroupCount { get; set; }
        public List<string> Teachers { get; set; }
        public List<string> Subjects { get; set; }
    }
}
