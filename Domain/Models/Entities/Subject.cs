using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Subject : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }        // e.g. "Calculus", "OOP"
        public int DepartmentId { get; set; }   // which department owns this subject
        public Department Department { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
