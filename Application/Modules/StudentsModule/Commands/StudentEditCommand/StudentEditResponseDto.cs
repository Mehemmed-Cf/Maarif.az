using Domain.Models.Stables;

namespace Application.Modules.StudentsModule.Commands.StudentEditCommand
{
    public class StudentEditResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FinCode { get; set; }
        public string StudentNumber { get; set; }
        public GenderType Gender { get; set; }
        public StatusType Status { get; set; }
        public byte Year { get; set; }
        public GradeType Grade { get; set; }
        public int DepartmentId { get; set; }
    }
}