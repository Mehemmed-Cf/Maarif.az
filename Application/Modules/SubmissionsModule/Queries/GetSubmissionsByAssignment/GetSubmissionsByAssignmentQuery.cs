using MediatR;
using System.Collections.Generic;

namespace Application.Modules.SubmissionsModule.Queries.GetSubmissionsByAssignment
{
    public class GetSubmissionsByAssignmentQuery : IRequest<List<SubmissionDto>>
    {
        public int AssignmentId { get; set; }
    }

    public class SubmissionDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public DateTime SubmissionDate { get; set; }
        // public int? Grade { get; set; }
    }
}
