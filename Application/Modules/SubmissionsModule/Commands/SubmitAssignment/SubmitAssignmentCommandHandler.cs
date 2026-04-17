using Application.Repositories;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.SubmissionsModule.Commands.SubmitAssignment
{
    public class SubmitAssignmentCommandHandler : IRequestHandler<SubmitAssignmentCommand, int>
    {
        private readonly ISubmissionRepository _repository;

        public SubmitAssignmentCommandHandler(ISubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(SubmitAssignmentCommand request, CancellationToken cancellationToken)
        {
            var submission = new Submission
            {
                AssignmentId = request.AssignmentId,
                StudentId = request.StudentId,
                SubmissionDate = DateTime.Now,
                // Apply your custom properties mapping
            };

            await _repository.AddAsync(submission, cancellationToken);
            await _repository.SaveAsync(cancellationToken);

            return submission.Id; 
        }
    }
}
