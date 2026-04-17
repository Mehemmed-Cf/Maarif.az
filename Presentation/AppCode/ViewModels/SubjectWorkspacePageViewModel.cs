using Application.Modules.SubjectsModule.Queries.PortalSubjectQuery;

namespace Presentation.AppCode.ViewModels
{
    public class SubjectWorkspacePageViewModel
    {
        public PortalSubjectWorkspaceDto Workspace { get; set; } = null!;
        public bool IsTeacher { get; set; }
    }
}
