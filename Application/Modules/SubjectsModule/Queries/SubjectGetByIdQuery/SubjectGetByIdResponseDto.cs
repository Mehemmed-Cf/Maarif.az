namespace Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery
{
    public class SubjectGetByIdResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SubjectDepartmentDto Department { get; set; }
        public IReadOnlyList<SubjectLessonDto> Lessons { get; set; }
    }

    public class SubjectDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FacultyName { get; set; }
    }

    public class SubjectLessonDto
    {
        public int Id { get; set; }
        public string TeacherFullName { get; set; }
    }
}