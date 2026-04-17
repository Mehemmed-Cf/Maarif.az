using Application.Repositories;
using MediatR;
using Infrastructure.Exceptions;

namespace Application.Modules.AssignmentsModule.Commands.GradeSubmission
{
    public class GradeSubmissionCommandHandler : IRequestHandler<GradeSubmissionCommand, bool>
    {
        private readonly ISubmissionRepository _repository;

        public GradeSubmissionCommandHandler(ISubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(GradeSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submission = await _repository.GetAsync(x => x.Id == request.SubmissionId, cancellationToken);
            if (submission == null) throw new NotFoundException("Submission not found");

            // You'll need properties like 'Grade' or 'TeacherFeedback' on your Submission entity
            // submission.Grade = request.Grade;
            // submission.ReviewFeedback = request.TeacherFeedback;
            
            await _repository.EditAsync(submission);
            await _repository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
