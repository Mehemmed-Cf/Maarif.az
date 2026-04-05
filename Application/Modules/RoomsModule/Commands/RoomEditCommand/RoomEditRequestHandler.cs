using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.RoomsModule.Commands.RoomEditCommand
{
    public class RoomEditRequestHandler : IRequestHandler<RoomEditRequest, RoomEditResponseDto>
    {
        private readonly IRoomRepository roomRepository;
        private readonly IBuildingRepository buildingRepository;
        private readonly IMapper mapper;

        public RoomEditRequestHandler(IRoomRepository roomRepository, IBuildingRepository buildingRepository, IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.buildingRepository = buildingRepository;
            this.mapper = mapper;
        }

        public async Task<RoomEditResponseDto> Handle(RoomEditRequest request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetAll(r => r.Id == request.Id && r.DeletedAt == null)
                .Include(r => r.Building)
                .FirstOrDefaultAsync(cancellationToken);

            if (room == null)
                throw new NotFoundException("Room not found.");

            mapper.Map(request, room);

            await roomRepository.EditAsync(room);
            await roomRepository.SaveAsync(cancellationToken);
            return mapper.Map<RoomEditResponseDto>(room);
        }
    }
}
