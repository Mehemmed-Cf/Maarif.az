using MediatR;

namespace Application.Modules.RoomsModule.Commands.RoomEditCommand
{
    public class RoomEditRequest : IRequest<RoomEditResponseDto>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int BuildingId { get; set; }
    }
}
