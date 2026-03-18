using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Teacher : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int GroupCount { get; set; }
        public int ActiveLessons { get; set; }
        public double Experience { get; set; }
        public int UserId { get; set; }
        public ICollection<TeacherDepartment> TeacherDepartments { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
