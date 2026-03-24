namespace Application.Modules.GroupsModule
{
    public class GroupLessonDto
    {
        public int LessonId { get; set; }
        // Assuming Lesson has a Subject or Name property you want to show
        public string SubjectName { get; set; }
    }
}
