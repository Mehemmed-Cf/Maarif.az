using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand
{
    public class DepartmentAddRequest : IRequest<DepartmentAddResponseDto>
    {
        public string Name { get; set; }
        public int FacultyId { get; set; }
        // FIX #8: Removed `public Faculty Faculty { get; set; }`.
        // Commands must never carry entity references — only primitive values (IDs, strings, etc.).
        // The handler resolves the Faculty from the DB if needed; the caller just sends FacultyId.
    }
}