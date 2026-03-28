using MediatR;

namespace Application.Modules.GroupsModule.Commands.GroupAddCommand
{
    public class GroupAddRequest : IRequest<GroupResponseDto>
    {
        public string Name { get; set; }
        public byte Year { get; set; }
        public int DepartmentId { get; set; }
        public List<int> StudentIds { get; set; } = new();
        public List<int> LessonIds { get; set; } = new();
    }
}
