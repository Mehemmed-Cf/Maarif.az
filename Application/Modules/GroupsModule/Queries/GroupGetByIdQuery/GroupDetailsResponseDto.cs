namespace Application.Modules.GroupsModule.Queries.GroupGetByIdQuery
{
    public class GroupDetailsResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Year { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; } //
        public List<StudentSmallDto> Students { get; set; }
        public List<LessonSmallDto> Lessons { get; set; }
    }
}
