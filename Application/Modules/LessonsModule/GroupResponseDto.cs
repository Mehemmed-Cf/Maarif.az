namespace Application.Modules.LessonsModule
{
    public class GroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Year { get; set; }
        // Adding these for better context and to fix compilation
        public string DepartmentName { get; set; }
        public int StudentCount { get; set; }
    }
}
