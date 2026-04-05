using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.RoomsModule.Commands.RoomRemoveCommand
{
    public class RoomRemoveRequestHandler : IRequestHandler<RoomRemoveRequest>
    {
        private readonly IRoomRepository roomRepository;

        public RoomRemoveRequestHandler(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public async Task Handle(RoomRemoveRequest request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetAsync(r => r.Id == request.Id && r.DeletedAt == null, cancellationToken);
            if (room == null)
                throw new NotFoundException("Room not found");

            roomRepository.Remove(room);
            await roomRepository.SaveAsync(cancellationToken);
        }
    }
}
