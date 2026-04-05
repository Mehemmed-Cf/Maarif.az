using MediatR;

namespace Application.Modules.RoomsModule.Commands.RoomAddCommand
{
    public class RoomAddRequest : IRequest<RoomAddResponseDto>
    {
        public int Number { get; set; }
        public int BuildingId { get; set; }
    }
}
