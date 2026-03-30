using MediatR;

namespace Application.Modules.GroupsModule.Commands.GroupEditCommand
{
    public class GroupEditRequest : IRequest<GroupResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Year { get; set; }
        public int DepartmentId { get; set; }
        public List<int> StudentIds { get; set; } = new();
        public List<int> LessonIds { get; set; } = new();
    }
}
