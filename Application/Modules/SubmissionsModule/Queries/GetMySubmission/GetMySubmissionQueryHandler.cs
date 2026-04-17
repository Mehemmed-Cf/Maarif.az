using Application.Repositories;
using MediatR;

namespace Application.Modules.SubmissionsModule.Queries.GetMySubmission
{
    public class GetMySubmissionQueryHandler : IRequestHandler<GetMySubmissionQuery, MySubmissionDto>
    {
        private readonly ISubmissionRepository _repository;

        public GetMySubmissionQueryHandler(ISubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<MySubmissionDto> Handle(GetMySubmissionQuery request, CancellationToken cancellationToken)
        {
            var submission = await _repository.GetAsync(x => x.AssignmentId == request.AssignmentId && x.StudentId == request.StudentId && x.DeletedAt == null, cancellationToken);

            if (submission == null) return null;

            return new MySubmissionDto
            {
                Id = submission.Id,
            };
        }
    }
}
