using Application.Repositories;
using MediatR;
using System.Linq;

namespace Application.Modules.SubmissionsModule.Queries.GetSubmissionsByAssignment
{
    public class GetSubmissionsByAssignmentQueryHandler : IRequestHandler<GetSubmissionsByAssignmentQuery, List<SubmissionDto>>
    {
        private readonly ISubmissionRepository _repository;

        public GetSubmissionsByAssignmentQueryHandler(ISubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SubmissionDto>> Handle(GetSubmissionsByAssignmentQuery request, CancellationToken cancellationToken)
        {
            var submissions = _repository
                .GetAll(x => x.AssignmentId == request.AssignmentId && x.DeletedAt == null)
                .ToList();

            // If you want to make this asynchronous, you can use Task.Run
            // var submissions = await Task.Run(() => _repository
            //     .GetAll(x => x.AssignmentId == request.AssignmentId && x.DeletedAt == null)
            //     .ToList(), cancellationToken);

            return submissions.Select(x => new SubmissionDto
            {
                Id = x.Id,
                StudentId = x.StudentId,
                SubmissionDate = x.SubmissionDate,
            }).ToList();
        }
    }
}
