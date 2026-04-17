using Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery;
using MediatR;

namespace Application.Modules.SubjectsModule.Queries.PortalSubjectQuery
{
    public class GetPortalSubjectWorkspaceRequest : IRequest<PortalSubjectWorkspaceDto>
    {
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public bool ForTeacher { get; set; }
    }

    public class PortalSubjectWorkspaceDto
    {
        public IReadOnlyList<PortalSubjectNavItemDto> NavSubjects { get; set; } = Array.Empty<PortalSubjectNavItemDto>();
        public SubjectGetByIdResponseDto Subject { get; set; } = null!;
    }
}
