namespace Application.Modules.RoomsModule.Commands.RoomEditCommand
{
    public class RoomEditResponseDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string DisplayName { get; set; }
    }
}
