using MediatR;
using System.Collections.Generic;

namespace Application.Modules.AssignmentsModule.Queries.GetAssignmentsByLesson
{
    public class GetAssignmentsByLessonQuery : IRequest<List<AssignmentDto>>
    {
        public int LessonId { get; set; }
    }

    public class AssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
