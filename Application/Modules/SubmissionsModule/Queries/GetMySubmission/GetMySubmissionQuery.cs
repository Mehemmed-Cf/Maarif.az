using MediatR;

namespace Application.Modules.SubmissionsModule.Queries.GetMySubmission
{
    public class GetMySubmissionQuery : IRequest<MySubmissionDto>
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
    }

    public class MySubmissionDto
    {
        public int Id { get; set; }
        // ... include submission details ...
    }
}
