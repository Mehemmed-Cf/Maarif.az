using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class Student : AuditableEntity
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
        public int UserId { get; set; }
        public GradeType Grade { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
