using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Faculty : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}
