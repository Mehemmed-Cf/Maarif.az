using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand
{
    public class DepartmentAddRequest : IRequest<DepartmentAddResponseDto>
    {
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
