using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.RoomsModule.Commands.RoomAddCommand
{
    public class RoomAddRequestHandler : IRequestHandler<RoomAddRequest, RoomAddResponseDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomAddRequestHandler(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<RoomAddResponseDto> Handle(RoomAddRequest request, CancellationToken cancellationToken)
        {
            var room = _mapper.Map<Room>(request);
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveAsync();

            // Reload with Building so AutoMapper can resolve BuildingName and DisplayName
            var roomWithBuilding = await _roomRepository.GetAll(r => r.Id == room.Id)
                .Include(r => r.Building)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<RoomAddResponseDto>(roomWithBuilding);
        }
    }
}
