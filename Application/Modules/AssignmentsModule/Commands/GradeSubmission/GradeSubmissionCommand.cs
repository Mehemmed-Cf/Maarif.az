using MediatR;

namespace Application.Modules.AssignmentsModule.Commands.GradeSubmission
{
    public class GradeSubmissionCommand : IRequest<bool>
    {
        public int SubmissionId { get; set; }
        public int Grade { get; set; }
        public string TeacherFeedback { get; set; }
    }
}
