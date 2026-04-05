using Application.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Modules.RoomsModule.Queries.RoomGetAllQuery
{
    public class RoomGetAllRequestHandler : IRequestHandler<RoomGetAllRequest, IEnumerable<RoomGetAllResponseDto>>
    {
        private readonly IRoomRepository roomRepository;
        private readonly IMapper mapper;

        public RoomGetAllRequestHandler(IRoomRepository roomRepository, IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RoomGetAllResponseDto>> Handle(RoomGetAllRequest request, CancellationToken cancellationToken)
        {
            var rooms = await roomRepository.GetAll(r => r.DeletedAt == null)
                .Include(r => r.Building)
                .ToListAsync(cancellationToken);

            return mapper.Map<IEnumerable<RoomGetAllResponseDto>>(rooms);
        }
    }
}
