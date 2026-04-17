using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Modules.AssignmentsModule.Commands.CreateAssignment
{
    public class CreateAssignmentCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int LessonId { get; set; }
        public DateTime DueDate { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
