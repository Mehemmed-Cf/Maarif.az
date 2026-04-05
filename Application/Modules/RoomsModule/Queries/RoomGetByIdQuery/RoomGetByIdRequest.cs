using MediatR;

namespace Application.Modules.RoomsModule.Queries.RoomGetByIdQuery
{
    public class RoomGetByIdRequest : IRequest<RoomGetByIdResponseDto>
    {
        public int Id { get; set; }
    }
}
