using Domain.Models.Stables;

namespace Application.Modules.StudentsModule.Queries.StudentGetAllQuery
{
    public class StudentGetAllResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string StudentNumber { get; set; }
        public GenderType Gender { get; set; }
        public string MobileNumber { get; set; }
        public EducationType EducationType { get; set; }
        public StatusType Status { get; set; }
        public byte Year { get; set; }
        public GradeType Grade { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
    }
}