namespace Application.Modules.RoomsModule.Commands.RoomAddCommand
{
    public class RoomAddResponseDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string DisplayName { get; set; }
    }
}
