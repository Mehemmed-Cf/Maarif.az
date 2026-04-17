using MediatR;

namespace Application.Modules.SubjectsModule.Queries.PortalSubjectQuery
{
    public class GetPortalSubjectNavRequest : IRequest<IReadOnlyList<PortalSubjectNavItemDto>>
    {
        public int UserId { get; set; }
        public bool ForTeacher { get; set; }
    }
}
