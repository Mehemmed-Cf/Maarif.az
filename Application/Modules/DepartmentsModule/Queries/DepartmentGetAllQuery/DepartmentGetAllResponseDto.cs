namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery
{
    public class DepartmentGetAllResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FacultyName { get; set; }
        public int StudentCount { get; set; }
        public int GroupCount { get; set; }
    }
}
