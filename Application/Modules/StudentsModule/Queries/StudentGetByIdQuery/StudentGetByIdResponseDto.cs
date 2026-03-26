using Domain.Models.Stables;

namespace Application.Modules.StudentsModule.Queries.StudentGetByIdQuery
{
    public class StudentGetByIdResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string StudentNumber { get; set; }
        public GenderType Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public EducationType EducationType { get; set; }
        public StatusType Status { get; set; }
        public byte Year { get; set; }
        public GradeType Grade { get; set; }
        public StudentDepartmentDto Department { get; set; }
        public IReadOnlyList<StudentGroupDto> Groups { get; set; }
    }

    public class StudentDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FacultyName { get; set; }
    }

    public class StudentGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Year { get; set; }
    }
}