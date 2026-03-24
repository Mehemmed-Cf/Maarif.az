namespace Application.Modules.GroupsModule
{
    public class GroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Year { get; set; }
        public string DepartmentName { get; set; } // Flattened from Department.Name
        public int StudentCount { get; set; } = 0;      // Calculated via AutoMapper
    }
}
