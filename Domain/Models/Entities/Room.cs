using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Room : AuditableEntity
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public int Floor => Number / 100;
    }
}
