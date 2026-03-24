namespace Application.Modules.LessonsModule
{
    public class LessonResponseDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string TeacherFullName { get; set; }
        public int GroupCount { get; set; }
    }
}
