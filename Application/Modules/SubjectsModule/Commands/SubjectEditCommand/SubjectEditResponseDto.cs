namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}