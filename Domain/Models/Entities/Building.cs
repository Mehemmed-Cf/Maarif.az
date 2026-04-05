using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
