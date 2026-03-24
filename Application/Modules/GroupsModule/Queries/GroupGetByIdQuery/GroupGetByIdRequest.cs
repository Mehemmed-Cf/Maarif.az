using MediatR;

namespace Application.Modules.GroupsModule.Queries.GroupGetByIdQuery
{
    public class GroupGetByIdRequest : IRequest<GroupDetailsResponseDto>
    {
        public int Id { get; set; }
    }
}
