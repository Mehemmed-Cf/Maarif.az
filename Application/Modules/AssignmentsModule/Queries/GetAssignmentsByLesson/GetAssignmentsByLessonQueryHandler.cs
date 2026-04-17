using Application.Repositories;
using MediatR;
using System.Linq;

namespace Application.Modules.AssignmentsModule.Queries.GetAssignmentsByLesson
{
    public class GetAssignmentsByLessonQueryHandler : IRequestHandler<GetAssignmentsByLessonQuery, List<AssignmentDto>>
    {
        private readonly IAssignmentRepository _repository;

        public GetAssignmentsByLessonQueryHandler(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AssignmentDto>> Handle(GetAssignmentsByLessonQuery request, CancellationToken cancellationToken)
        {
            var assignments = _repository
                .GetAll(x => x.LessonId == request.LessonId && x.DeletedAt == null)
                .ToList();

            // If you want to make this asynchronous, use Task.Run to avoid blocking
            // var assignments = await Task.Run(() => _repository
            //     .GetAll(x => x.LessonId == request.LessonId && x.DeletedAt == null)
            //     .ToList(), cancellationToken);

            return assignments.Select(x => new AssignmentDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate
            }).ToList();
        }
    }
}
