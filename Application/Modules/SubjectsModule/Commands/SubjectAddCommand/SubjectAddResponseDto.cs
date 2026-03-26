namespace Application.Modules.SubjectsModule.Commands.SubjectAddCommand
{
    public class SubjectAddResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}