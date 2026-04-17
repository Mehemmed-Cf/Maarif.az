using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Department : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public ICollection<TeacherDepartment> TeacherDepartments { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}
