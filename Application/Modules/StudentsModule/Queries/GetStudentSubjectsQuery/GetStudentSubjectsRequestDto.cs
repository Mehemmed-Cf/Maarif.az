namespace Application.Modules.StudentsModule.Queries.GetStudentSubjectsQuery
{
    public class GetStudentSubjectsRequestDto
    {
        public List<StudentSubjectDto> Subjects { get; set; } = new();
    }

    public class StudentSubjectDto
    {
        public int SubjectId { get; set; }
    }
}
