using MediatR;

namespace Application.Modules.GroupsModule.Queries.GroupGetAllQuery
{
    public class GroupGetAllRequest : IRequest<IEnumerable<GroupResponseDto>>
    {
    }
}
