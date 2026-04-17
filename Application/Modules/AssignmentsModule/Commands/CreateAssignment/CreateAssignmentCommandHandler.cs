using Application.Repositories;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.AssignmentsModule.Commands.CreateAssignment
{
    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, int>
    {
        private readonly IAssignmentRepository _repository;

        // Note: You might want to inject a file service here to handle IFormFile uploads
        public CreateAssignmentCommandHandler(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = new Assignment
            {
                Title = request.Title,
                Description = request.Description,
                LessonId = request.LessonId,
                DueDate = request.DueDate,
                // Handle properties based on your entity
            };

            await _repository.AddAsync(assignment);
            await _repository.SaveAsync(cancellationToken);

            return assignment.Id;
        }
    }
}
