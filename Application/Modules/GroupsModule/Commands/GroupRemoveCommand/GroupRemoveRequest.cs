using MediatR;

namespace Application.Modules.GroupsModule.Commands.GroupRemoveCommand
{
    public class GroupRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
