namespace Application.Modules.RoomsModule.Queries.RoomGetByIdQuery
{
    public class RoomGetByIdResponseDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string DisplayName { get; set; }
    }
}
