namespace Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery
{
    public class SubjectGetAllResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
        public int LessonCount { get; set; }
    }
}