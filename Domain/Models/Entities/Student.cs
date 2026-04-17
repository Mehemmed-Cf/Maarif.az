using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class Student : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string FinCode { get; set; } // new
        /// <summary>Normalized identity document serial from self-registration (AZE…); null for admin-created rows.</summary>
        public string? DocumentSerialNumber { get; set; }
        public string StudentNumber { get; set; }
        public GenderType Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public EducationType EducationType { get; set; }
        public StatusType Status { get; set; }
        public byte Year { get; set; }
        public int UserId { get; set; }
        public GradeType Grade { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // BUG FIX: Removed FacultyId — Faculty is already reachable via Department.Faculty.
        // Storing it here was a 3NF violation that would cause data drift if a
        // department is ever reassigned to a different faculty.
        // Use: student.Department.Faculty or join through Department in queries.

        public ICollection<StudentGroup> StudentGroups { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}