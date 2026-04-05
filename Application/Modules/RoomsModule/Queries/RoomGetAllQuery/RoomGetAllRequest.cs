using MediatR;
using System.Collections.Generic;

namespace Application.Modules.RoomsModule.Queries.RoomGetAllQuery
{
    public class RoomGetAllRequest : IRequest<IEnumerable<RoomGetAllResponseDto>>
    {
    }
}
