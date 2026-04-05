using MediatR;

namespace Application.Modules.RoomsModule.Commands.RoomRemoveCommand
{
    public class RoomRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
