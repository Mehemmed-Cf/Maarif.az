using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Modules.SubmissionsModule.Commands.SubmitAssignment
{
    public class SubmitAssignmentCommand : IRequest<int>
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string Note { get; set; }
        public string RepositoryLink { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
