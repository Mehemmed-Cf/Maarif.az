using Domain.Models.Stables;

namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentStudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public GenderType Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public EducationType EducationType { get; set; }
        public StatusType Status { get; set; }
        public byte Year { get; set; }
        public GradeType Grade { get; set; }
        // FIX #3: Removed FacultyId — no longer stored on Student.
        // Faculty is reachable via Student.Department.Faculty when needed.
    }
}