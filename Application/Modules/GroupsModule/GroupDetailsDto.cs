namespace Application.Modules.GroupsModule
{
    public class GroupDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Year { get; set; }
        public int DepartmentId { get; set; }

        // Linked Students
        public List<GroupStudentDto> Students { get; set; }
        // NEW: Linked Lessons
        public List<GroupLessonDto> Lessons { get; set; }
    }
}
