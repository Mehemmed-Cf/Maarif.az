namespace Application.Modules.LessonsModule
{
    public class LessonDetailResponseDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public string TeacherFullName { get; set; }
        public int TeacherId { get; set; }
        public IReadOnlyList<GroupResponseDto> Groups { get; set; }
    }
}
