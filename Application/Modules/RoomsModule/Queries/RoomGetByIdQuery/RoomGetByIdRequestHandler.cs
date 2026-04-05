using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Modules.RoomsModule.Queries.RoomGetByIdQuery
{
    public class RoomGetByIdRequestHandler : IRequestHandler<RoomGetByIdRequest, RoomGetByIdResponseDto>
    {
        private readonly IRoomRepository roomRepository;
        private readonly IMapper mapper;

        public RoomGetByIdRequestHandler(IRoomRepository roomRepository, IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.mapper = mapper;
        }

        public async Task<RoomGetByIdResponseDto> Handle(RoomGetByIdRequest request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetAll(r => r.Id == request.Id && r.DeletedAt == null)
                .Include(r => r.Building)
                .FirstOrDefaultAsync(cancellationToken);

            if (room == null)
                throw new NotFoundException("Room not found.");

            return mapper.Map<RoomGetByIdResponseDto>(room);
        }
    }
}
