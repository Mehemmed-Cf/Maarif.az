namespace Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery
{
    public class FacultyGetByIdResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // This list will contain only the ID and Name for each department
        public IReadOnlyList<FacultyDepartmentDto> Departments { get; set; }
    }
}
